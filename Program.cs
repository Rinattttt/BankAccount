using System;
using System.Threading;

class Program
{
    static void Main()
    {
        var account = new Account();//Создание объекта класса Account
        const decimal requiredAmount = 100m;// Сумма, позволяющая снимать деньги со счета
        // Создание нового потока для цикличного пополнения счета случайными значениями 
        var depositThread = new Thread(() =>
        {
            var random = new Random();
            for (int i = 0; i < 3; i++)//Пополнение будет происходить 3 раза случаныйми значениями
            {
                {
                    account.Deposit(random.Next(10, 60));
                    Thread.Sleep(1000);//Задержка между заполнениями
                }
            }
        });

        depositThread.Start();//Запуск потока

        account.WaitForBalance(requiredAmount);// Проверка на возможность снятия средств

        if (account.TryWithdraw(requiredAmount)) //Снятие средств
        {
            Console.WriteLine($"Остаток на счете: " + account.balance);
        }
    }
}

    class Account
    {
        public decimal balance; //Поле для баланса
        private readonly object balanceLock = new(); //Параметр для оператора lock

        public Account()//Конструктор
        {
            this.balance = 0;
        }

        public void Deposit(decimal amount)//Метод для пополнения
        {
            lock (balanceLock)
            {
                balance += amount;
                Console.WriteLine($"Пополнение: {amount:C}. Баланс: {balance:C}");//Спец. формат для денежных значений
            }
        }

        public void WaitForBalance(decimal requiredAmount)//Метод для проверки на возможность снятия средств
        {
            while (true)
            {
                lock (balanceLock)
                {
                if (balance >= requiredAmount)
                {
                    Console.WriteLine("Достаточно средств для снятия.");
                    return;
                }
                }
                Thread.Sleep(500);// Подождать перед следующей проверкой
            }
        }

        public bool TryWithdraw(decimal amount)//Проверочный метод для снятия средств
        {
            lock (balanceLock)
            {
                if (balance >= amount)
                {
                    balance -= amount;
                    Console.WriteLine($"Снятие: {amount:C}. Баланс: {balance:C}");
                    return true;
                }
                Console.WriteLine("Недостаточно средств.");
                return false;
            }
        }
    }




