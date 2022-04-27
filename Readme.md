### Создание структуры проекта
##### 1. Создал проект WPF, desctop, Core 6.0 с именем проекта ( UI )
##### 2. Создал библиотеку, Core 6.0 с именем проекта ( DAL )
### БД
##### 1. В слой DAL, через nuget установил Npgsql.EntityFrameworkCore.PostgreSQL версии 6,0,4
##### 2. В слой DAL, через nuget установил Microsoft.EntityFrameworkCore.Tools версии 6,0,4
##### 3. В слой UI, через nuget установил Microsoft.EntityFrameworkCore.Design версии 6,0,4
##### 4. Создал базовую сущность с Id тип Guid
>Тонкости для использования Guid в качестве первичного ключа
>> На поле которое будет первичным ключем нужно повесить следующий атрибут
>> [DatabaseGenerated(DatabaseGeneratedOption.None)]
>> 
>> Т.к. инкрементировать его БД не будет.
>
>> Кроме того, учитывая, что в данном случае значение для этого поля
>> нужно генерировать самостоятельно, желательно поместить его генерацию в конструктор

```
    public class BaseEntity
    {
        public BaseEntity()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; }
    }
```
##### 5. Создал две сущности Shift и CashVoucher унаследованные от BaseEntity
```
    public class Shift : BaseEntity
    {
        [DisplayName("Номер смены")]
        public int Number { get; set; }

        //[InverseProperty("Shift")]
        public virtual ICollection<CashVoucher> CashVouchers { get; set; }
    }
```
```
    public class CashVoucher : BaseEntity
    {
        [DisplayName("Номер чека")]
        public int Number { get; set; }

        [DisplayName("ID смены")]
        public Guid? ShiftId { get; set; }

        //[InverseProperty("CashVouchers")]
        public virtual Shift Shift { get; set; }
    }
```
##### 6. Создал класс контекста БД
##### 7. Добавил в класс контекста БД два ДБСета для сущностей и сам контекст со строкой подключения
    public class ApplicationContext : DbContext
    {
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<CashVoucher> CashVouchers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=TestDB_ForTestWPF;Username=postgres;Password=Prikhodkoas123321");
        }
    }
##### 8. Открыл Консоль диспетчера пакетов (выбрал в выпадающем списке проект DAL) и выполнил Add-Migration Initial
>Альтернативный способ
>>
>> Открыть PowerShell
>> 
>> Перейти в каталог с DAL проектом
>> 
>> Выполнить dotnet ef migrations add Initial

    Создалась миграция
##### 9. Открыл Консоль диспетчера пакетов (выбрал в выпадающем списке проект DAL) и выполнил Update-Database
>Альтернативный способ
>>
>> Открыть PowerShell
>>
>> Перейти в каталог с DAL проектом
>>
>> Выполнить dotnet ef database update

    В БД появились таблицы соответствующие сущностям
### Внедрение патерна Repository (Generic)
##### 1. Создал generic интерфейс для основных CRUD операций
```
    internal interface IRootRepository<T> where T : BaseEntity
    {
        public IQueryable<T> GetAll();
        public T GetById(Guid id);
        public Guid Insert(T entity);
        public void Update(T entity);
        public bool DeleteById(Guid id);
    }
```
##### 2. Создал интерфейсы для сущностей проекта
```
    public interface IShiftsRepository
    {
        IQueryable<Shift> GetAll();
        Shift GetById(Guid id);
        Guid Insert(Shift entity);
        void Update(Shift entity);
        bool DeleteById(Guid id);

        Shift GetShiftByNumber(int number);
    }
```

```
    public interface ICashVouchersRepository
    {
        IQueryable<CashVoucher> GetAll();
        CashVoucher GetById(Guid id);
        Guid Insert(CashVoucher entity);
        void Update(CashVoucher entity);
        bool DeleteById(Guid id);
    }
```
##### 3. Реализавал generic интерфейс для основных CRUD операций
```
    public class RootRepository<T> : IRootRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationContext _context;
        private readonly DbSet<T> _entities;

        public RootRepository()
        {
            _context = new ApplicationContext();
            _entities = _context.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return _entities;
            // return _context.Set<T>
        }

        public T GetById(Guid id)
        {
            return _entities.FirstOrDefault(p => p.Id == id);
            //return _context.Set<T>.FirstOrDefault(p => p.Id == id);
        }

        public Guid Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _entities.Add(entity);
            //_context.Set<T>.Add(entity);

            _context.SaveChanges();
            return entity.Id;
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
        }

        public bool DeleteById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            var entity = GetById(id);

            if (entity != null)
            {
                try
                {
                    _entities.Remove(entity);
                    //_context.Set<T>.Remove(entity);

                    _context.SaveChanges();
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }
    }
```
##### 4. Реализовал интерфейсы сущностей (нужно просто отнаследоваться от RootRepository\<T\> и дописать или переопределить недостающие или уникальные операции)
```
    public class ShiftsRepository : RootRepository<Shift>, IShiftsRepository
    {
        public Shift GetShiftByNumber(int number)
        {
            return _context.Shifts.FirstOrDefault(p => p.Number == number);
        }
    }
```

>Для того что бы при запросе всех чеков, возвращались так же и смены внутри объектов CashVoucher. Необходимо добавить их при выборке. В данном случае используется жадная загрузка с использованием Include.

```
    public class CashVouchersRepository : RootRepository<CashVoucher>, ICashVouchersRepository
    {
        public IQueryable<CashVoucher> GetAll()
        {
            return _context.CashVouchers.Include(p => p.Shift);
        }

        public Guid Insert(CashVoucher cashVoucher)
        {
            if (cashVoucher == null)
            {
                throw new ArgumentNullException(nameof(cashVoucher));
            }

            if (cashVoucher.Shift == null)
            {
                throw new ArgumentNullException(nameof(cashVoucher.Shift));
            }

            cashVoucher.Shift = _context.Shifts.FirstOrDefault(p => p.Id == cashVoucher.Shift.Id);

            _context.CashVouchers.Add(cashVoucher);
            _context.SaveChanges();

            return cashVoucher.Id;
        }
    }
```