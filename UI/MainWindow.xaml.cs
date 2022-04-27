using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DAL.Entities;
using DAL.Services;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButAddShift_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tBoxShiftNumberFromAddShift.Text))
            {
                MessageBox.Show("Необходимо ввести номер смены", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int number;
            if (!int.TryParse(tBoxShiftNumberFromAddShift.Text, out number))
            {
                MessageBox.Show("Введите цифровой номер смены", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var shiftsRepository = new ShiftsRepository();

            if (shiftsRepository.GetShiftByNumber(number) != null)
            {
                MessageBox.Show($"Смена номер {number} уже существует. Введите другой номер", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                shiftsRepository.Insert(new Shift
                {
                    Number = number
                });

                MessageBox.Show($"Смена номер {number} успешно добавлена.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Не удалось добавить новую смену в БД", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButAddCheque_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tBoxShiftNumberFromAddCheque.Text) ||
                string.IsNullOrEmpty(tBoxNumberCheque.Text))
            {
                MessageBox.Show("Необходимо ввести номер смены и номер чека", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int shiftNumber;
            if (!int.TryParse(tBoxShiftNumberFromAddCheque.Text, out shiftNumber))
            {
                MessageBox.Show("Введите цифровой номер смены", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int chequeNumber;
            if (!int.TryParse(tBoxNumberCheque.Text, out chequeNumber))
            {
                MessageBox.Show("Введите цифровой номер чека", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var shiftsRepository = new ShiftsRepository();

            var shift = shiftsRepository.GetShiftByNumber(shiftNumber);
            if (shift == null)
            {
                MessageBox.Show($"Смена номер {shiftNumber} не существует. Введите другой номер смены", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var cashVouchersRepository = new CashVouchersRepository();
                cashVouchersRepository.Insert(new CashVoucher
                {
                    Shift = shift,
                    Number = chequeNumber
                });

                MessageBox.Show($"Чек номер {chequeNumber} успешно добавлен.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Не удалось добавить новый чек в БД", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            
        }

        private void ButGetAllCheques_OnClick(object sender, RoutedEventArgs e)
        {
            var cashVouchersRepository = new CashVouchersRepository();

            var newTextConsole = new StringBuilder();
            foreach (var cashVoucher in cashVouchersRepository.GetAll())
            {
                newTextConsole.Append($"Чек номер {cashVoucher.Number} в смене {cashVoucher.Shift.Number}\n");
            }

            tBoxConsole.Text = newTextConsole.ToString();
        }
    }
}
