### Создание структуры проекта
##### 1. Создал проект WPF, desctop, Core 6.0 с именем проекта ( UI )
##### 2. Создал библиотеку, Core 6.0 с именем проекта ( DAL )
### БД
##### 1. В слой DAL, через nuget установил Npgsql.EntityFrameworkCore.PostgreSQL версии 6,0,4
##### 2. В слой DAL, через nuget установил Microsoft.EntityFrameworkCore.Tools версии 6,0,4
##### 3. В слой UI, через nuget установил Microsoft.EntityFrameworkCore.Design версии 6,0,4
##### 4. Создал две сущности Shift и CashVoucher
```
    public class Shift
    {
        public int Id { get; set; }

        [DisplayName("Номер смены")]
        public int Number { get; set; }

        //[InverseProperty("Shift")]
        public virtual ICollection<CashVoucher> CashVouchers { get; set; }
    }
```
```
    public class Shift
    {
        public int Id { get; set; }

        [DisplayName("Номер смены")]
        public int Number { get; set; }

        //[InverseProperty("Shift")]
        public virtual ICollection<CashVoucher> CashVouchers { get; set; }
    }
```
##### 5. Создал класс контекста БД
##### 6. Добавил в класс контекста БД два ДБСета для сущностей и сам контекст со строкой подключения
    public class ApplicationContext : DbContext
    {
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<CashVoucher> CashVouchers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=TestDB_ForTestWPF;Username=postgres;Password=Prikhodkoas123321");
        }
    }
##### 7. Открыл Консоль диспетчера пакетов (выбрал в выпадающем списке проект DAL) и выполнил Add-Migration Initial
>Альтернативный способ
>>
>> Открыть PowerShell
>> 
>> Перейти в каталог с DAL проектом
>> 
>> Выполнить dotnet ef migrations add Initial

    Создалась миграция
##### 8. Открыл Консоль диспетчера пакетов (выбрал в выпадающем списке проект DAL) и выполнил Update-Database
>Альтернативный способ
>>
>> Открыть PowerShell
>>
>> Перейти в каталог с DAL проектом
>>
>> Выполнить dotnet ef database update

    В БД появились таблицы соответствующие сущностям