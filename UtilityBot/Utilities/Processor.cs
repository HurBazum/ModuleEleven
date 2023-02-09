using Telegram.Bot.Types.Enums;

namespace UtilityBot.Utilities
{
    public static class Processor
    {
        public static string TextProcessing(string command, string message)
        {
            string result = string.Empty;

            if (command != string.Empty)
            {
                if (message != null)
                {
                    switch (command)
                    {
                        case "/length":
                            result = new string($"Длина \"{message}\" - {message.Length} знаков.");
                            break;
                        case "/sum":
                            result = CalcSumDigitInString(message);
                            break;
                    }
                }
            }
            else
            {
                result = $"Выберете команду в главном меню, \"/start\"," +
                    $"\nили введите \"/length\" - считает длину сообщения," +
                    $"\n\"/sum\" - считает сумму введённых чисел";
            }

            return result;
        }

        static string CalcSumDigitInString(string input) 
        {
            try
            {
                string[] inputMass = input.Split(' ');
                int result = 0;

                for(int i = 0; i < inputMass.Length; i++)
                {
                    if (int.TryParse(inputMass[i], out int a))
                    {
                        if (i == 0)
                        {
                            inputMass[i] = a.ToString();
                        }
                        else
                        {
                            if (a > 0)
                            {
                                inputMass[i] = " + " + a.ToString();
                            }
                            else
                            {
                                inputMass[i] = " - " + ((-1) * a).ToString();
                            }
                        }
                        if(result == 0)
                        {
                            result = a;
                        }
                        else
                        {
                            result += a;
                        }
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                return string.Concat(inputMass) + " = " + result.ToString();
            }
            catch 
            {
                return $"<u>Ой! Что-то было введено неверно!</u>\nНапоминаю: <b><i>мы складываем числа, для этого их нужно вводить через пробел!</i></b>";
            }
        }
    }
}