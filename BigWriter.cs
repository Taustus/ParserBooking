using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserBooking
{
    public class BigWriter
    {
        Application _ObjExcel;
        Workbook _ObjWorkBook;
        Worksheet _ObjWorkSheet;

        int _startCell = 4;
        int _types = 0;
        int _offers = 0;
        string _checkin = null;
        string _checkout = null;
        int _breakfastConst = 0;
        int _includesConst = 0;
        int _priceConst = 0;
        int _daysCount = 0;

        public BigWriter(string checkin, string checkout, int types, int offers)
        {
            _ObjExcel = new Application();
            _ObjExcel.Visible = true;
            _ObjWorkBook = _ObjExcel.Workbooks.Open(Directory.GetCurrentDirectory() + @"\Data.xlsx");

            _ObjWorkSheet = (Worksheet)_ObjWorkBook.Sheets[_ObjWorkBook.Sheets.Count];
            try
            {
                if (!(_ObjWorkSheet.Cells[4, 1] as Range).Value.ToString().Equals(""))
                {
                    _ObjWorkBook.Sheets.Add(After: _ObjWorkBook.Sheets[_ObjWorkBook.Sheets.Count]);
                }
            }
            catch { }
            _ObjWorkSheet = (Worksheet)_ObjWorkBook.Sheets[_ObjWorkBook.Sheets.Count];


            _ObjWorkSheet.Cells[3, 1] = "№";
            _ObjWorkSheet.Cells[3, 2] = "Property Booking.com id";
            _ObjWorkSheet.Cells[3, 3] = "Property Booking.com URL";
            _ObjWorkSheet.Cells[3, 4] = "Property name";
            _ObjWorkSheet.Cells[3, 5] = "Property type";
            _ObjWorkSheet.Cells[3, 6] = "Star Rating";
            _ObjWorkSheet.Cells[3, 7] = "Booking.com score";
            _ObjWorkSheet.Cells[3, 8] = "Booking.com reviews count";
            _ObjWorkSheet.Cells[3, 9] = "City";
            _ObjWorkSheet.Cells[3, 10] = "Region";
            _ObjWorkSheet.Cells[3, 11] = "Address";
            _ObjWorkSheet.Cells[3, 12] = "Spa presence";
            _ObjWorkSheet.Cells[3, 13] = "Дата начала";
            _ObjWorkSheet.Cells[3, 14] = "Дата окончания";


            _types = types;
            _offers = offers;
            _checkin = checkin;
            _checkout = checkout;
            _daysCount = Convert.ToInt16(checkout.Split('.')[0]) - Convert.ToInt16(checkin.Split('.')[0]);

            for (int i = 0; i < types; i++)
            {
                _ObjWorkSheet.Cells[3, 15 + i] = $"{i + 1} тип";
            }
            int counter = 0;
            //Creating fields
            for (int l = 0; l < 7; l++)
            {
                switch (l)
                {
                    case 0:
                        {
                            _ObjWorkSheet.Cells[1, 15 + types + counter] = "Цены за выбранный период";
                            _priceConst = 15 + types + counter;
                            break;
                        }
                    case 1:
                        {
                            _ObjWorkSheet.Cells[1, 15 + types + counter] = "Вмещает(кол-во гостей) для соответствующего предложения";
                            break;
                        }
                    case 2:
                        {
                            _ObjWorkSheet.Cells[1, 15 + types + counter] = "Какое питание включено для соответствующего предложения";
                            _includesConst = 15 + types + counter;
                            break;
                        }
                    case 3:
                        {
                            _ObjWorkSheet.Cells[1, 15 + types + counter] = "Оплата возвращается";
                            break;
                        }
                    case 4:
                        {
                            _ObjWorkSheet.Cells[1, 15 + types + counter] = "Завтрак";
                            _breakfastConst = 15 + types + counter;
                            _ObjWorkSheet.Cells[1, 15 + types + counter + 1] = "90%";
                            _ObjWorkSheet.Cells[1, 15 + types + counter + 2] = "3";
                            _ObjWorkSheet.Cells[1, 15 + types + counter + 3] = "80%";

                            _ObjWorkSheet.Cells[2, 15 + types + counter] = "Повыш.на номера";
                            _ObjWorkSheet.Cells[2, 15 + types + counter + 1] = "10%";
                            _ObjWorkSheet.Cells[2, 15 + types + counter + 2] = "4";
                            _ObjWorkSheet.Cells[2, 15 + types + counter + 3] = "80%";

                            _ObjWorkSheet.Cells[3, 15 + types + counter + 2] = "5";
                            _ObjWorkSheet.Cells[3, 15 + types + counter + 3] = "75%";
                            counter += 4;
                            _ObjWorkSheet.Cells[3, 15 + types + counter++] = "Кол-во ночей";
                            continue;
                        }
                    case 5:
                        {
                            _ObjWorkSheet.Cells[1, 15 + types + counter] = "Цена очищенная от завтрака (10%)";
                            break;
                        }
                    case 6:
                        {
                            _ObjWorkSheet.Cells[1, 15 + types + counter] = "ADR, руб./сут без НДС";
                            break;
                        }
                    default:
                        break;
                }
                for (int i = 0; i < types; i++)
                {
                    for (int k = 0; k < offers; k++)
                    {
                        _ObjWorkSheet.Cells[3, 15 + types + counter] = $"{k + 1} предложение";
                        _ObjWorkSheet.Cells[2, 15 + types + counter++] = $"{i + 1} тип";
                    }
                }
            }
        }

        public void Write(Property prop)
        {
            _ObjWorkSheet.Cells[_startCell, 1] = _startCell - 3;
            _ObjWorkSheet.Cells[_startCell, 2] = prop.ID;
            _ObjWorkSheet.Cells[_startCell, 3] = prop.URL;
            _ObjWorkSheet.Cells[_startCell, 4] = prop.Name;
            _ObjWorkSheet.Cells[_startCell, 5] = prop.Type;
            _ObjWorkSheet.Cells[_startCell, 6] = prop.Stars;
            _ObjWorkSheet.Cells[_startCell, 7] = prop.Rating;
            _ObjWorkSheet.Cells[_startCell, 8] = prop.Reviews;
            _ObjWorkSheet.Cells[_startCell, 9] = prop.City;
            _ObjWorkSheet.Cells[_startCell, 10] = prop.Region;
            _ObjWorkSheet.Cells[_startCell, 11] = prop.Address;
            _ObjWorkSheet.Cells[_startCell, 12] = prop.SPA;

            _ObjWorkSheet.Cells[_startCell, 13] = _checkin;
            _ObjWorkSheet.Cells[_startCell, 14] = _checkout;
            _ObjWorkSheet.Cells[_startCell, _breakfastConst + 4] = _daysCount;

            for (int i = 0; i < _types && i < prop.Rooms.Count; i++)
            {
                _ObjWorkSheet.Cells[_startCell, 15 + i] = prop.Rooms[i].Roomtype;
            }
            var counter = 0;
            for (int l = 0; l < 4; l++)
            {
                for (int i = 0; i < _types; i++)
                {
                    if (i >= prop.Rooms.Count)
                    {
                        counter += _offers;
                    }
                    else
                    {
                        for (int k = 0; k < _offers; k++)
                        {
                            if (k >= prop.Rooms[i].Price.Count)
                            {
                                counter++;
                            }
                            else
                            {
                                switch (l)
                                {
                                    case 0:
                                        {
                                            _ObjWorkSheet.Cells[_startCell, 15 + _types + counter++] = prop.Rooms[i].Price[k];
                                            break;
                                        }
                                    case 1:
                                        {
                                            _ObjWorkSheet.Cells[_startCell, 15 + _types + counter++] = prop.Rooms[i].Capacity[k];
                                            break;
                                        }
                                    case 2:
                                        {
                                            _ObjWorkSheet.Cells[_startCell, 15 + _types + counter++] = prop.Rooms[i].Meal[k];
                                            break;
                                        }
                                    case 3:
                                        {
                                            _ObjWorkSheet.Cells[_startCell, 15 + _types + counter++] = prop.Rooms[i].Refund[k];
                                            break;
                                        }
                                }
                            }
                        }
                    }
                }
            }
            //formulas
            counter = 0;
            var another = 0;
            for (int l = 0; l < 2; l++)
            {
                for (int i = 0; i < _types; i++)
                {
                    for (int k = 0; k < _offers; k++)
                    {
                        switch (l)
                        {
                            case 0:
                                {
                                    //=IF(T5="нет",P5,IF(IFERROR(SEARCH("ужин",T5),0)+IFERROR(SEARCH("обед",T5),0)>0,"",IF(ISNUMBER(SEARCH("завтрак",T5)),P5*$Y$1,"")))
                                    string str = $"=IF({ColumnName(_includesConst + counter) + _startCell}=\"нет\",{ColumnName(_priceConst + counter) + _startCell},IF(IFERROR(SEARCH(\"ужин\",{ColumnName(_includesConst + counter) + _startCell}),0)" +
                                        $"+IFERROR(SEARCH(\"обед\",{ColumnName(_includesConst + counter) + _startCell}),0)>0,\"\",IF(ISNUMBER(SEARCH(\"завтрак\",{ColumnName(_includesConst + counter) + _startCell}" +
                                        $")),{ColumnName(_priceConst + counter) + _startCell}*{ColumnName(_breakfastConst + 1) + 1},\"\")))";
                                    _ObjWorkSheet.Range[ColumnName(_breakfastConst + 5 + counter++) + _startCell].Formula = str;
                                    break;
                                }
                            case 1:
                                {
                                    string str = $"=IFERROR(IF($F{_startCell}>=3,VLOOKUP($F{_startCell},{ColumnName(_breakfastConst + 2) + 1}:{ColumnName(_breakfastConst + 3) + 3},2,FALSE)*" +
                                        $"{ColumnName(_breakfastConst + 5 + another++) + _startCell}*(1+{ColumnName(_breakfastConst + 1) + 2})/1.2/{ColumnName(_breakfastConst + 4) + _startCell},\"\"),\"\")";
                                    _ObjWorkSheet.Range[ColumnName(_breakfastConst + 5 + counter++) + _startCell].Formula = str;

                                    break;
                                }
                            default:
                                break;
                        }
                    }
                }
            }
            _startCell++;
        }

        private string ColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }

        public void Finish()
        {
            try
            {
                _ObjWorkBook.Close(SaveChanges: true);
                _ObjExcel.Quit();
            }
            catch
            {

            }
        }
    }
}

