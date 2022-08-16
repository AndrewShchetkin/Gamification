using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gamification.Validators
{
    public class ExcelParserValidator
    {
        public string Error {get; set;}

        public ExcelParserValidator()
        {
            Error = "Ошибка обработки Excel файла: ";
        }
        public bool ValidateColumns(int cols)
        {
            if (cols != 6) // max columns in whole table
            {
                Error += "В строке должно быть 6 колонок!";
                return false;
            }
            return true;
        }

        public bool ValidateColumnNullRef(object colValue, int col, int row)
        {
            if (colValue == null)
            {
                Error += $"Пустая ячейка ({row + 1} строка, {col + 1} колонка)";
                return false;
            }
            return true;
        }

        public bool ValidateColumnIsNum(object colValue, int col, int row)
        {
            try
            {
               Int32.Parse(colValue.ToString());
               return true;
            }
            catch (FormatException ex)
            {
                Error += $"В ячейке должно быть число ({row + 1} строка, {col + 1} колонка)";
                return false;
            }
        }

        public bool ValidateEmptyFile(int rows)
        {
            if (rows == 0)
            {
                Error += "Файл пустой!";
                return false;
            }
            return true;
        }
    }
}
