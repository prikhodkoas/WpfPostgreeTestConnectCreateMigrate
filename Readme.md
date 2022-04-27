### �������� ��������� �������
##### 1. ������ ������ WPF, desctop, Core 6.0 � ������ ������� ( UI )
##### 2. ������ ����������, Core 6.0 � ������ ������� ( DAL )
### ��
##### 1. � ���� DAL, ����� nuget ��������� Npgsql.EntityFrameworkCore.PostgreSQL ������ 6,0,4
##### 2. � ���� DAL, ����� nuget ��������� Microsoft.EntityFrameworkCore.Tools ������ 6,0,4
##### 3. � ���� UI, ����� nuget ��������� Microsoft.EntityFrameworkCore.Design ������ 6,0,4
##### 4. ������ ������� �������� � Id ��� Guid
>�������� ��� ������������� Guid � �������� ���������� �����
>> �� ���� ������� ����� ��������� ������ ����� �������� ��������� �������
>> [DatabaseGenerated(DatabaseGeneratedOption.None)]
>> 
>> �.�. ���������������� ��� �� �� �����.
>
>> ����� ����, ��������, ��� � ������ ������ �������� ��� ����� ����
>> ����� ������������ ��������������, ���������� ��������� ��� ��������� � �����������

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
##### 5. ������ ��� �������� Shift � CashVoucher �������������� �� BaseEntity
```
    public class Shift : BaseEntity
    {
        [DisplayName("����� �����")]
        public int Number { get; set; }

        //[InverseProperty("Shift")]
        public virtual ICollection<CashVoucher> CashVouchers { get; set; }
    }
```
```
    public class CashVoucher : BaseEntity
    {
        [DisplayName("����� ����")]
        public int Number { get; set; }

        [DisplayName("ID �����")]
        public Guid? ShiftId { get; set; }

        //[InverseProperty("CashVouchers")]
        public virtual Shift Shift { get; set; }
    }
```
##### 6. ������ ����� ��������� ��
##### 7. ������� � ����� ��������� �� ��� ������ ��� ��������� � ��� �������� �� ������� �����������
    public class ApplicationContext : DbContext
    {
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<CashVoucher> CashVouchers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=TestDB_ForTestWPF;Username=postgres;Password=Prikhodkoas123321");
        }
    }
##### 8. ������ ������� ���������� ������� (������ � ���������� ������ ������ DAL) � �������� Add-Migration Initial
>�������������� ������
>>
>> ������� PowerShell
>> 
>> ������� � ������� � DAL ��������
>> 
>> ��������� dotnet ef migrations add Initial

    ��������� ��������
##### 9. ������ ������� ���������� ������� (������ � ���������� ������ ������ DAL) � �������� Update-Database
>�������������� ������
>>
>> ������� PowerShell
>>
>> ������� � ������� � DAL ��������
>>
>> ��������� dotnet ef database update

    � �� ��������� ������� ��������������� ���������
### ��������� ������� Repository (Generic)
##### 1. ������ generic ��������� ��� �������� CRUD ��������
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
##### 2. ������ ���������� ��� ��������� �������
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
##### 3. ���������� generic ��������� ��� �������� CRUD ��������
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
##### 4. ���������� ���������� ��������� (����� ������ ��������������� �� RootRepository\<T\> � �������� ��� �������������� ����������� ��� ���������� ��������)
```
    public class ShiftsRepository : RootRepository<Shift>, IShiftsRepository
    {
        public Shift GetShiftByNumber(int number)
        {
            return _context.Shifts.FirstOrDefault(p => p.Number == number);
        }
    }
```

>��� ���� ��� �� ��� ������� ���� �����, ������������ ��� �� � ����� ������ �������� CashVoucher. ���������� �������� �� ��� �������. � ������ ������ ������������ ������ �������� � �������������� Include.

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