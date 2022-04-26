### �������� ��������� �������
##### 1. ������ ������ WPF, desctop, Core 6.0 � ������ ������� ( UI )
##### 2. ������ ����������, Core 6.0 � ������ ������� ( DAL )
### ��
##### 1. � ���� DAL, ����� nuget ��������� Npgsql.EntityFrameworkCore.PostgreSQL ������ 6,0,4
##### 2. � ���� DAL, ����� nuget ��������� Microsoft.EntityFrameworkCore.Tools ������ 6,0,4
##### 3. � ���� UI, ����� nuget ��������� Microsoft.EntityFrameworkCore.Design ������ 6,0,4
##### 4. ������ ��� �������� Shift � CashVoucher
```
    public class Shift
    {
        public int Id { get; set; }

        [DisplayName("����� �����")]
        public int Number { get; set; }

        //[InverseProperty("Shift")]
        public virtual ICollection<CashVoucher> CashVouchers { get; set; }
    }
```
```
    public class Shift
    {
        public int Id { get; set; }

        [DisplayName("����� �����")]
        public int Number { get; set; }

        //[InverseProperty("Shift")]
        public virtual ICollection<CashVoucher> CashVouchers { get; set; }
    }
```
##### 5. ������ ����� ��������� ��
##### 6. ������� � ����� ��������� �� ��� ������ ��� ��������� � ��� �������� �� ������� �����������
    public class ApplicationContext : DbContext
    {
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<CashVoucher> CashVouchers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=TestDB_ForTestWPF;Username=postgres;Password=Prikhodkoas123321");
        }
    }
##### 7. ������ ������� ���������� ������� (������ � ���������� ������ ������ DAL) � �������� Add-Migration Initial
>�������������� ������
>>
>> ������� PowerShell
>> 
>> ������� � ������� � DAL ��������
>> 
>> ��������� dotnet ef migrations add Initial

    ��������� ��������
##### 8. ������ ������� ���������� ������� (������ � ���������� ������ ������ DAL) � �������� Update-Database
>�������������� ������
>>
>> ������� PowerShell
>>
>> ������� � ������� � DAL ��������
>>
>> ��������� dotnet ef database update

    � �� ��������� ������� ��������������� ���������