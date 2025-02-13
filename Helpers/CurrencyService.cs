using e_commerce.Enums;

namespace e_commerce.Helpers
{
    public class CurrencyService
    {
        // Function to get currency symbol by code
        public static string GetCurrencySymbolByCode(string currencyCode)
        {
            // Currency Dictionary for all major countries
            return currencyCode switch
            {
                "AFN" => "؋",    // Afghan Afghani
                "ALL" => "L",    // Albanian Lek
                "DZD" => "د.ج",  // Algerian Dinar
                "USD" => "$",    // United States Dollar
                "EUR" => "€",    // Euro
                "AOA" => "Kz",   // Angolan Kwanza
                "ARS" => "$",    // Argentine Peso
                "AUD" => "A$",   // Australian Dollar
                "AZN" => "₼",    // Azerbaijani Manat
                "BDT" => "৳",    // Bangladeshi Taka
                "BHD" => ".د.ب", // Bahraini Dinar
                "BRL" => "R$",   // Brazilian Real
                "GBP" => "£",    // British Pound Sterling
                "CAD" => "C$",   // Canadian Dollar
                "CNY" => "¥",    // Chinese Yuan
                "COP" => "$",    // Colombian Peso
                "EGP" => "£",    // Egyptian Pound
                "INR" => "₹",    // Indian Rupee
                "IDR" => "Rp",   // Indonesian Rupiah
                "JPY" => "¥",    // Japanese Yen
                "KRW" => "₩",    // South Korean Won
                "MYR" => "RM",   // Malaysian Ringgit
                "MXN" => "$",    // Mexican Peso
                "NGN" => "₦",    // Nigerian Naira
                "PKR" => "₨",    // Pakistani Rupee
                "PHP" => "₱",    // Philippine Peso
                "RUB" => "₽",    // Russian Ruble
                "SAR" => "﷼",    // Saudi Riyal
                "SGD" => "S$",   // Singapore Dollar
                "ZAR" => "R",    // South African Rand
                "LKR" => "Rs",   // Sri Lankan Rupee
                "SEK" => "kr",   // Swedish Krona
                "CHF" => "CHF",  // Swiss Franc
                "THB" => "฿",    // Thai Baht
                "TRY" => "₺",    // Turkish Lira
                "UAH" => "₴",    // Ukrainian Hryvnia
                "AED" => "د.إ",  // UAE Dirham
                "VND" => "₫",    // Vietnamese Dong
                "PLN" => "zł",   // Polish Zloty
                "NOK" => "kr",   // Norwegian Krone
                "CZK" => "Kč",   // Czech Koruna
                "HKD" => "HK$",  // Hong Kong Dollar
                "HUF" => "Ft",   // Hungarian Forint
                "ISK" => "kr",   // Icelandic Krona
                "KES" => "Sh",   // Kenyan Shilling
                "KWD" => "د.ك",  // Kuwaiti Dinar
                "NZD" => "NZ$",  // New Zealand Dollar
                "QAR" => "﷼",    // Qatari Riyal
                "RON" => "lei",  // Romanian Leu
                "TWD" => "NT$",  // New Taiwan Dollar
                "ZMW" => "ZK",   // Zambian Kwacha
                _ => "৳"         // Default case for invalid or unknown currencies
            };
        }

        public static string GetCurrencyCodeBySymbol(string symbol)
        {
            return symbol switch
            {
                "؋" => "AFN",     // Afghan Afghani
                "L" => "ALL",     // Albanian Lek
                "د.ج" => "DZD",   // Algerian Dinar
                "$" => "USD",     // United States Dollar (You can handle multiple currencies here)
                "€" => "EUR",     // Euro
                "Kz" => "AOA",    // Angolan Kwanza
                "A$" => "AUD",    // Australian Dollar
                "₼" => "AZN",     // Azerbaijani Manat
                "৳" => "BDT",     // Bangladeshi Taka
                ".د.ب" => "BHD",  // Bahraini Dinar
                "R$" => "BRL",    // Brazilian Real
                "£" => "GBP",     // British Pound Sterling
                "C$" => "CAD",    // Canadian Dollar
                "¥" => "CNY",     // Chinese Yuan
                "₨" => "PKR",     // Pakistani Rupee
                "₽" => "RUB",     // Russian Ruble
                "﷼" => "SAR",     // Saudi Riyal
                "S$" => "SGD",     // Singapore Dollar
                "R" => "ZAR",     // South African Rand
                "Rs" => "LKR",    // Sri Lankan Rupee
                "kr" => "SEK",    // Swedish Krona
                "CHF" => "CHF",   // Swiss Franc
                "฿" => "THB",     // Thai Baht
                "₺" => "TRY",     // Turkish Lira
                "₴" => "UAH",     // Ukrainian Hryvnia
                "د.إ" => "AED",   // UAE Dirham
                "₫" => "VND",     // Vietnamese Dong
                "zł" => "PLN",    // Polish Zloty
                "Kč" => "CZK",    // Czech Koruna
                "HK$" => "HKD",   // Hong Kong Dollar
                "Ft" => "HUF",    // Hungarian Forint
                "Sh" => "KES",    // Kenyan Shilling
                "د.ك" => "KWD",   // Kuwaiti Dinar
                "NZ$" => "NZD",   // New Zealand Dollar
                "lei" => "RON",   // Romanian Leu
                "NT$" => "TWD",   // New Taiwan Dollar
                "ZK" => "ZMW",    // Zambian Kwacha
                "ARS" => "$",     // Argentine Peso
                "COP" => "$",     // Colombian Peso
                "EGP" => "£",     // Egyptian Pound
                "INR" => "₹",     // Indian Rupee
                "IDR" => "Rp",    // Indonesian Rupiah
                "JPY" => "¥",     // Japanese Yen
                "KRW" => "₩",     // South Korean Won
                "MYR" => "RM",    // Malaysian Ringgit
                "MXN" => "$",     // Mexican Peso
                "NGN" => "₦",     // Nigerian Naira
                "PHP" => "₱",     // Philippine Peso
                "QAR" => "﷼",     // Qatari Riyal
                _ => "BDT"        // Default case for invalid or unknown currency symbols
            };
        }


    }
}
