﻿using BusinessLogicLayer.DataTransferObjects;
using BusinessLogicLayer.Services;

namespace BusinessLogicLayer
{
    public class ConverterHandler
    {
        private  MinfinParser minfinParser = new MinfinParser("http://minfin.com.ua/currency/banks/");
        private IuaParser iuaParser = new IuaParser("http://finance.i.ua/bank/4/");

        public float [,] GetMatrix (int selectBank)
        {
            if (selectBank == 1)
            {
                return iuaParser.CurrencyTable;
            }
            else
            {
                return minfinParser.CurrencyTable;
            }
        }
       
        public float CalculateValue(float value, Currency srcCurrency, Currency dstCurrency, Operation operation, Bank selectedBank)
        {
            
            bool fromUAH = srcCurrency == Currency.UAH;
            bool toUAH = dstCurrency == Currency.UAH;

            if (selectedBank == Bank.Aval)
            {
                if (fromUAH)
                {
                    return value / iuaParser.CurrencyTable[1 - (int) operation, (int) dstCurrency];
                }
                else if (toUAH)
                {
                    return value * iuaParser.CurrencyTable[(int) operation, (int) srcCurrency];
                }
                else
                {
                    float uah = CalculateValue(value, srcCurrency, Currency.UAH, operation, selectedBank);
                    return CalculateValue(uah, Currency.UAH, dstCurrency, operation, selectedBank);
                }
            }
            else
            {
                if (fromUAH)
                {
                    return value / minfinParser.CurrencyTable[1 - (int)operation, (int)dstCurrency];
                }
                else if (toUAH)
                {
                    return value * minfinParser.CurrencyTable[(int)operation, (int)srcCurrency];
                }
                else
                {
                    float uah = CalculateValue(value, srcCurrency, Currency.UAH, operation, selectedBank);
                    return CalculateValue(uah, Currency.UAH, dstCurrency, operation, selectedBank);
                }
            }
        }
    }
}
