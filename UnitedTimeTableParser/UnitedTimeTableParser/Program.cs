using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDFReader;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Data.SqlClient;
using System.Data;

namespace UnitedTimeTableParser
{
    public class Program
    {
        [Serializable]
        public class CIFLight
        {
            // Auto-implemented properties. 

            public string FromIATA;
            public string ToIATA;
            public DateTime FromDate;
            public DateTime ToDate;
            public Boolean FlightMonday;
            public Boolean FlightTuesday;
            public Boolean FlightWednesday;
            public Boolean FlightThursday;
            public Boolean FlightFriday;
            public Boolean FlightSaterday;
            public Boolean FlightSunday;
            public DateTime DepartTime;
            public DateTime ArrivalTime;
            public String FlightNumber;
            public String FlightAirline;
            public String FlightOperator;
            public String FlightAircraft;
            public Boolean FlightCodeShare;
            public Boolean FlightNextDayArrival;
            public int FlightNextDays;
            public string FlightDuration;
        }

        public class AirlinesFlightNumbers
        {
            // Auto-implemented properties. 
            public int From { get; set; }
            public int To { get; set; }
            public string Airline { get; set; }
        }

        public static readonly List<string> _UnitedAircraftCode = new List<string>() { "100", "318", "319", "320", "321", "32A", "32S", "330", "332", "333", "340", "343", "345", "346", "380", "388", "717", "733", "734", "735", "736", "737", "738", "739", "73C", "73G", "73H", "73J", "73W", "744", "747", "74E", "74H", "752", "753", "757", "762", "763", "764", "767", "772", "773", "777", "77L", "77W", "787", "788", "789", "AR1", "AR8", "AT5", "AT7", "ATR", "BE1", "BEH", "BNI", "BUS", "CNA", "CR1", "CR2", "CR7", "CR9", "CRA", "CRJ", "CRK", "DH1", "DH2", "DH3", "DH4", "DH8", "E70", "E75", "E7W", "E90", "E95", "EM2", "EMJ", "EQV", "ER3", "ER4", "ERD", "ERJ", "F70", "ICE", "M80", "M90", "S20", "SF3", "TRN" };
        public static readonly List<string> _LufthansaAirlineCode = new List<string>() { "LH", "LX", "2L", "9L", "A3", "AC", "AF", "AI", "AV", "AX", "B6", "BE", "C3", "CA", "CL", "CO", "EN", "ET", "EV", "EW", "F7", "G7", "IQ", "JJ", "JP", "K2", "KM", "LG", "LO", "LY", "MS", "NH", "NI", "NZ", "OL", "OO", "OS", "OU", "OZ", "PS", "PT", "QI", "QR", "S5", "SA", "SK", "SN", "SQ", "TA", "TG", "TK", "TP", "UA", "US", "VO", "WK", "YV", "2A" };
        static List<AirlinesFlightNumbers> _Airlines = new List<AirlinesFlightNumbers>
        {
            new AirlinesFlightNumbers {From = 1, To = 1299, Airline = "United Airlines"},
            new AirlinesFlightNumbers {From = 1340, To = 1344, Airline = "ExpressJet-Extra Section"},
            new AirlinesFlightNumbers {From = 1400, To = 1764, Airline = "United Airlines"},
            new AirlinesFlightNumbers {From = 2050, To = 2119, Airline = "UA Extra Sections - (XP)"},
            new AirlinesFlightNumbers {From = 2193, To = 2200, Airline = "Cargo Extra Section (XF)"},
            new AirlinesFlightNumbers {From = 2900, To = 2949, Airline = "Cape Air (Carribean)"},
            new AirlinesFlightNumbers {From = 2950, To = 3049, Airline = "Great Lakes Codeshare"},
            new AirlinesFlightNumbers {From = 3050, To = 3149, Airline = "Silver Airways Codeshare"},
            new AirlinesFlightNumbers {From = 3150, To = 3249, Airline = "Amtrak"},
            new AirlinesFlightNumbers {From = 3250, To = 3254, Airline = "Commutair Extra Section"},
            new AirlinesFlightNumbers {From = 3255, To = 3274, Airline = "UAX - ExpressJet - CO Contract"},
            new AirlinesFlightNumbers {From = 3275, To = 3294, Airline = "Skywest Extra Sections"},
            new AirlinesFlightNumbers {From = 3325, To = 3329, Airline = "Trans States Extra Sections"},
            new AirlinesFlightNumbers {From = 3330, To = 3434, Airline = "UAX-Trans States"},
            new AirlinesFlightNumbers {From = 3435, To = 3439, Airline = "Shuttle America Extra Sections"},
            new AirlinesFlightNumbers {From = 3440, To = 3584, Airline = "UAX-Shuttle America"},
            new AirlinesFlightNumbers {From = 3585, To = 3590, Airline = "UAX-CommutAir DH3 - CO Contract"},
            new AirlinesFlightNumbers {From = 3591, To = 3604, Airline = "UAX-CommutAir DH2 - CO Contract"},
            new AirlinesFlightNumbers {From = 3605, To = 3714, Airline = "UAX-Go Jet"},
            new AirlinesFlightNumbers {From = 3715, To = 3717, Airline = "Go Jet Extra Sections"},
            new AirlinesFlightNumbers {From = 3720, To = 3724, Airline = "Mesa Extra Sections"},
            new AirlinesFlightNumbers {From = 3725, To = 3804, Airline = "UAX-Mesa"},
            new AirlinesFlightNumbers {From = 3805, To = 3854, Airline = "UAX-Expressjet"},
            new AirlinesFlightNumbers {From = 3855, To = 3904, Airline = "UAX-Republic - UA Contract"},
            new AirlinesFlightNumbers {From = 3905, To = 3919, Airline = "UAX-Shuttle America"},
            new AirlinesFlightNumbers {From = 3920, To = 3965, Airline = "UAX-ExpressJet - CO Contract"},
            new AirlinesFlightNumbers {From = 3966, To = 3969, Airline = "Shuttle America Extra Sections"},
            new AirlinesFlightNumbers {From = 3970, To = 3973, Airline = "Silver Airways - Extra Sections"},
            new AirlinesFlightNumbers {From = 4051, To = 4066, Airline = "UAX- Silver Airways - IAD"},
            new AirlinesFlightNumbers {From = 4067, To = 4084, Airline = "UAX - Silver Airways - CLE"},
            new AirlinesFlightNumbers {From = 4085, To = 4769, Airline = "UAX - ExpressJet - CO Contract"},
            new AirlinesFlightNumbers {From = 4770, To = 4834, Airline = "UAX-CommutAir DH2 - CO Contract"},
            new AirlinesFlightNumbers {From = 4835, To = 4859, Airline = "UAX-CommutAir DH3 - CO Contract"},
            new AirlinesFlightNumbers {From = 4860, To = 4868, Airline = "UAX-ExpressJet - CO Contract"},
            new AirlinesFlightNumbers {From = 4869, To = 4947, Airline = "UAX-Republic - UA Contract"},
            new AirlinesFlightNumbers {From = 4953, To = 4955, Airline = "UAX-Republic - Extra Sections"},
            new AirlinesFlightNumbers {From = 4965, To = 4999, Airline = "UAX-Skywest"},
            new AirlinesFlightNumbers {From = 5000, To = 5021, Airline = "UAX-CommutAir DH2 - CO Contract"},
            new AirlinesFlightNumbers {From = 5022, To = 5037, Airline = "UAX- Trans States"},
            new AirlinesFlightNumbers {From = 5038, To = 5082, Airline = "UAX - Cape Air GUM"},
            new AirlinesFlightNumbers {From = 5083, To = 5083, Airline = "UAX Cape Air - Extra Section"},
            new AirlinesFlightNumbers {From = 5090, To = 5122, Airline = "UAX-Mesa"},
            new AirlinesFlightNumbers {From = 5123, To = 5155, Airline = "UAX-Shuttle America"},
            new AirlinesFlightNumbers {From = 5156, To = 5269, Airline = "UAX-Skywest"},
            new AirlinesFlightNumbers {From = 5270, To = 5289, Airline = "UAX- Trans States"},
            new AirlinesFlightNumbers {From = 5290, To = 5659, Airline = "UAX-Skywest"},
            new AirlinesFlightNumbers {From = 5660, To = 5684, Airline = "UAX - ExpressJet - CO Contract"},
            new AirlinesFlightNumbers {From = 5685, To = 5774, Airline = "UAX-ExpressJet - former ASA"},
            new AirlinesFlightNumbers {From = 5775, To = 5778, Airline = "ExpressJet - former ASA Extra Sections"},
            new AirlinesFlightNumbers {From = 5779, To = 5809, Airline = "UAX-ExpressJet - CO Contract"},
            new AirlinesFlightNumbers {From = 5810, To = 5899, Airline = "UAX-Expressjet"},
            new AirlinesFlightNumbers {From = 5900, To = 6189, Airline = "UAX-ExpressJet - CO Contract"},
            new AirlinesFlightNumbers {From = 6190, To = 6539, Airline = "UAX- Skywest Airlines"},
            new AirlinesFlightNumbers {From = 6540, To = 6544, Airline = "UAX - Bus Service - EWR-ABE"},
            new AirlinesFlightNumbers {From = 6545, To = 6548, Airline = "UAX - Bus Service - IAH-BPT"},
            new AirlinesFlightNumbers {From = 6549, To = 6598, Airline = "Aeromar"},
            new AirlinesFlightNumbers {From = 6629, To = 6748, Airline = "Avianca"},
            new AirlinesFlightNumbers {From = 6749, To = 6828, Airline = "Air New Zealand"},
            new AirlinesFlightNumbers {From = 6829, To = 6848, Airline = "LOT-Polish Airlines"},
            new AirlinesFlightNumbers {From = 6974, To = 7073, Airline = "SAS - Scandinavian Airlines"},
            new AirlinesFlightNumbers {From = 7074, To = 7173, Airline = "Copa Airlines"},
            new AirlinesFlightNumbers {From = 7174, To = 7223, Airline = "Ethiopian Airlines"},
            new AirlinesFlightNumbers {From = 7224, To = 7253, Airline = "South African Airways"},
            new AirlinesFlightNumbers {From = 7254, To = 7283, Airline = "Croatia Airlines"},
            new AirlinesFlightNumbers {From = 7284, To = 7323, Airline = "Asiana"},
            new AirlinesFlightNumbers {From = 7424, To = 7623, Airline = "Air China"},
            new AirlinesFlightNumbers {From = 7624, To = 7668, Airline = "Aer Lingus"},
            new AirlinesFlightNumbers {From = 7669, To = 7768, Airline = "Swiss"},
            new AirlinesFlightNumbers {From = 7769, To = 7792, Airline = "Turkish Airlines"},
            new AirlinesFlightNumbers {From = 7793, To = 7878, Airline = "Hawaiian Airlines"},
            new AirlinesFlightNumbers {From = 7879, To = 7914, Airline = "IslandAir"},
            new AirlinesFlightNumbers {From = 8015, To = 8714, Airline = "Air Canada"},
            new AirlinesFlightNumbers {From = 8715, To = 9589, Airline = "Lufthansa Airlines"},
            new AirlinesFlightNumbers {From = 9590, To = 9639, Airline = "TAP-Air Portugal"},
            new AirlinesFlightNumbers {From = 9640, To = 9739, Airline = "ANA-All Nippon"},
            new AirlinesFlightNumbers {From = 9815, To = 9854, Airline = "Austrian Codeshare"},
            new AirlinesFlightNumbers {From = 9855, To = 9868, Airline = "Egypt Air"},
            new AirlinesFlightNumbers {From = 9883, To = 9899, Airline = "EVA Air"},
            new AirlinesFlightNumbers {From = 9900, To = 9998, Airline = "Brussels Airlines"}
        };

        static void Main(string[] args)
        {

            var text = new StringBuilder();
            CultureInfo ci = new CultureInfo("en-US");
            string path = AppDomain.CurrentDomain.BaseDirectory + "data\\united.pdf";
            Regex rgxtime = new Regex(@"(1[0-2]|[1-9]):[0-5][0-9](a|p|A|P)(\+1)?");
            Regex rgxFlightNumber = new Regex(@" \d{1,4} ");            
            Regex rgxIATAAirport = new Regex(@"\([A-Z]{3}");
            Regex rgxFlightDaysMonth = new Regex(@"(S|-)(S|-)(M|-)(T|-)(W|-)(T|-)(F|-)\|(S|-)(S|-)(M|-)(T|-)(W|-)(T|-)(F|-)\|(S|-)(S|-)(M|-)(T|-)(W|-)(T|-)(F|-)\|(S|-)(S|-)(M|-)(T|-)(W|-)(T|-)(F|-)$");
            Regex rgxFlightDaysWeek = new Regex(@"(S|-)(S|-)(M|-)(T|-)(W|-)(T|-)(F|-)");
            Regex rgxdate1 = new Regex(@"(([0-9])|([0-2][0-9])|([3][0-1])) (Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)");            
            Regex rgxFlightTime = new Regex(@"(([0-9]|0[0-9]|1[0-9]|2[0-3])h)?([0-9]|0[0-9]|1[0-9]|2[0-9]|3[0-9]|4[0-9]|5[0-9])m");
            Regex rgxTimeZone = new Regex(@"^(?:Z|[+-](?:2[0-3]|[01][0-9]):[0-5][0-9])$");
            Regex rgxAircraftCodes = new Regex(string.Join("|", _UnitedAircraftCode), RegexOptions.Compiled);
            Regex rgxFlightDistance = new Regex(@"(\d|,)*\d* mi");
            List<CIFLight> CIFLights = new List<CIFLight> { };
            List<Rectangle> rectangles = new List<Rectangle>();

            //rectangles.Add(new Rectangle(x+(j*offset), (y+i*offset), offset, offset));
            float distanceInPixelsFromLeft = 0;
            float distanceInPixelsFromBottom = 30;
            float width = 250;//pdfReader.GetPageSize(page).Width / 2; // 306 deelt niet naar helft? 
            float height = 560; // pdfReader.GetPageSize(page).Height;
            // Formaat papaier 
            // Letter		 612x792
            // A4		     595x842
            var left = new Rectangle(
                        distanceInPixelsFromLeft,
                        distanceInPixelsFromBottom,
                        width,
                        height);

            var center = new Rectangle(
                       300,
                       distanceInPixelsFromBottom,
                       450,
                       height);

            var right = new Rectangle(
                       550,
                       distanceInPixelsFromBottom,
                       612,
                       height);


            rectangles.Add(left);
            rectangles.Add(center);
            rectangles.Add(right);

            // The PdfReader object implements IDisposable.Dispose, so you can
            // wrap it in the using keyword to automatically dispose of it
            Console.WriteLine("Opening PDF File...");

            //PdfReader reader = new PdfReader(path);


            using (var pdfReader = new PdfReader(path))
            {
                // Vaststellen valid from to date
                DateTime ValidFrom = new DateTime(2015, 6, 20);
                DateTime ValidTo = new DateTime(2015, 7, 18);

                // Vaststellen van Basics

                string TEMP_FromIATA = null;
                string TEMP_ToIATA = null;
                DateTime TEMP_ValidFrom = new DateTime();
                DateTime TEMP_ValidTo = new DateTime();
                int TEMP_Conversie = 0;
                Boolean TEMP_FlightMonday = false;
                Boolean TEMP_FlightTuesday = false;
                Boolean TEMP_FlightWednesday = false;
                Boolean TEMP_FlightThursday = false;
                Boolean TEMP_FlightFriday = false;
                Boolean TEMP_FlightSaterday = false;
                Boolean TEMP_FlightSunday = false;
                DateTime TEMP_DepartTime = new DateTime();
                DateTime TEMP_ArrivalTime = new DateTime();
                Boolean TEMP_FlightCodeShare = false;
                string TEMP_FlightNumber = null;
                string TEMP_Aircraftcode = null;
                TimeSpan TEMP_DurationTime = TimeSpan.MinValue;
                Boolean TEMP_FlightNextDayArrival = false;
                int TEMP_FlightNextDays = 0;
                string TEMP_FlightOperator = null;               
                // Loop through each page of the document
                for (var page = 15; page <= pdfReader.NumberOfPages; page++)
                //for (var page = 3; page <= pdfReader.NumberOfPages; page++)
                {

                    Console.WriteLine("Parsing page {0}...", page);
                    float pageHeight = pdfReader.GetPageSize(page).Height;
                    float pageWidth = pdfReader.GetPageSize(page).Width;

                    //System.Diagnostics.Debug.WriteLine(currentText);


                    foreach (Rectangle rect in rectangles)
                    {
                        ITextExtractionStrategy its = new CSVTextExtractionStrategy();
                        var filters = new RenderFilter[1];
                        filters[0] = new RegionTextRenderFilter(rect);
                        //filters[1] = new RegionTextRenderFilter(rechts);

                        ITextExtractionStrategy strategy =
                            new FilteredTextRenderListener(
                                new CSVTextExtractionStrategy(), // new LocationTextExtractionStrategy()
                                filters);

                        var currentText = PdfTextExtractor.GetTextFromPage(
                            pdfReader,
                            page,
                            strategy);

                        currentText =
                            Encoding.UTF8.GetString(Encoding.Convert(
                                Encoding.Default,
                                Encoding.UTF8,
                                Encoding.Default.GetBytes(currentText)));

                        string[] lines = Regex.Split(currentText, "\r\n");

                        foreach (string line in lines)
                        {
                            string[] values = line.SplitWithQualifier(',', '\"', true);
                             // New To:
                            if (rgxIATAAirport.IsMatch(line) && rgxFlightDistance.IsMatch(line))
                            {
                                // New To
                                TEMP_ToIATA = null;
                                TEMP_ToIATA = rgxIATAAirport.Match(line).Groups[0].Value.Replace("(", "");
                            }
                            if (rgxIATAAirport.IsMatch(line) && !rgxFlightDistance.IsMatch(line))
                            {
                                // New From 
                                TEMP_FromIATA = null;
                                TEMP_FromIATA = rgxIATAAirport.Match(line).Groups[0].Value.Replace("(", "");
                            }

                            foreach (string value in values)
                            {
                                if (!String.IsNullOrEmpty(value.Trim()))
                                {
                                    // getrimde string temp value
                                    string temp_string = value.Trim();

                                    // assuming C#
                                    //if (temp_string == "OS376")
                                    //{
                                    //System.Diagnostics.Debugger.Break();
                                    //}

                                   
                                    
                                                                        
                                    // From en To
                                    //if (rgxIATAAirport.Matches(temp_string).Count > 0)
                                    //{
                                        
                                    //    if (String.IsNullOrEmpty(TEMP_FromIATA))
                                    //    {
                                    //        TEMP_FromIATA = rgxIATAAirport.Match(temp_string).Groups[0].Value.Replace("(", "");
                                    //    }
                                    //    else
                                    //    {
                                    //        if (String.IsNullOrEmpty(TEMP_ToIATA) && !String.IsNullOrEmpty(TEMP_FromIATA))
                                    //        {
                                    //            TEMP_ToIATA = rgxIATAAirport.Match(temp_string).Groups[0].Value.Replace("(", "");
                                    //        }
                                    //    }
                                    //}
                                    //if (temp_string == "®")
                                    //{
                                    //    // New To airport.
                                    //    TEMP_ToIATA = null;
                                    //}

                                    if (rgxFlightDaysMonth.Matches(temp_string).Count > 0)
                                    {
                                        int dayset = 0;
                                        // it the line with the route information
                                        if (rgxFlightDaysWeek.Matches(temp_string).Count > 0) {
                                            // Parse the individual week                                            
                                            foreach (Match ItemMatch in rgxFlightDaysWeek.Matches(temp_string))
                                            {
                                                // Week from to 

                                                if (dayset == 0)
                                                {
                                                    TEMP_ValidFrom = ValidFrom;
                                                    TEMP_ValidTo = ValidTo.AddDays(-21);
                                                }
                                                if (dayset == 1)
                                                {
                                                    TEMP_ValidFrom = ValidFrom.AddDays(7);
                                                    TEMP_ValidTo = ValidTo.AddDays(-14);
                                                }
                                                if (dayset == 2)
                                                {
                                                    TEMP_ValidFrom = ValidFrom.AddDays(14);
                                                    TEMP_ValidTo = ValidTo.AddDays(-7);
                                                }
                                                if (dayset == 3)
                                                {
                                                    TEMP_ValidFrom = ValidFrom.AddDays(21);
                                                    TEMP_ValidTo = ValidTo;
                                                }
                                                
                                                string y = ItemMatch.Value;
                                                // Aircraft parsing  
                                                TEMP_Aircraftcode = rgxAircraftCodes.Match(temp_string).Groups[0].Value;
                                                TEMP_FlightNumber = "UA" + rgxFlightNumber.Match(temp_string).Groups[0].Value.Trim();
                                                int flightnumber = Convert.ToInt32(rgxFlightNumber.Match(temp_string).Groups[0].Value.Trim());
                                                if (flightnumber >= 1299) { TEMP_FlightCodeShare = true; }
                                                // Airline Parsing
                                                try
                                                {
                                                    var Airline = _Airlines.Find(item => (item.From <= flightnumber) && (item.To >= flightnumber)).Airline.ToString();
                                                    TEMP_FlightOperator = Airline;
                                                }
                                                catch
                                                {
                                                    // Oops, that was exceptional.
                                                    TEMP_FlightOperator = @"United Airlines - Unknown";
                                                }
                                                
                                                // Flight Time Parsing
                                                foreach (Match ItemTimeMatch in rgxtime.Matches(temp_string)) 
                                                {
                                                    string x = ItemTimeMatch.Value;
                                                    x = x.Replace("A", " AM");
                                                    x = x.Replace("P", " PM");
                                                    if (TEMP_DepartTime == DateTime.MinValue)
                                                        {
                                                            // tijd parsing                                                
                                                            //DateTime.TryParse(x, out TEMP_DepartTime);
                                                            TEMP_DepartTime = DateTime.ParseExact(x, "h:mm tt", new System.Globalization.CultureInfo("en-US"), DateTimeStyles.None);
                                                            //DateTime.TryParseExact(temp_string, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out TEMP_DepartTime);
                                                        }
                                                    else
                                                        {
                                                            // Er is al een waarde voor from dus dit is de to.                                                           
                                                            if (x.Contains("+1"))
                                                            {
                                                                // Next day arrival
                                                                x = x.Replace("+1", "");
                                                                TEMP_FlightNextDays = 1;
                                                                TEMP_FlightNextDayArrival = true;
                                                            }
                                                            TEMP_ArrivalTime = DateTime.ParseExact(x, "h:mm tt", new System.Globalization.CultureInfo("en-US"), DateTimeStyles.None); 
                                                        }
                                                }
                                                // Flight day parsing.
                                                if (y[0].ToString() == "S") { TEMP_FlightSaterday = true; }
                                                if (y[1].ToString() == "S") { TEMP_FlightSunday = true; }
                                                if (y[2].ToString() == "M") { TEMP_FlightMonday = true; }
                                                if (y[3].ToString() == "T") { TEMP_FlightTuesday = true; }
                                                if (y[4].ToString() == "W") { TEMP_FlightWednesday = true; }
                                                if (y[5].ToString() == "T") { TEMP_FlightThursday = true; }
                                                if (y[6].ToString() == "F") { TEMP_FlightFriday = true; }
                                                // Duration parsing
                                                if (rgxFlightTime.Matches(temp_string).Count > 0)
                                                {
                                                    int intFlightTimeH = 0; 
                                                    int intFlightTimeM = 0; 
                                                    var match = rgxFlightTime.Match(temp_string);                                                    
                                                    if (rgxFlightTime.Match(temp_string).Value.Contains("h"))
                                                    {
                                                        string matchgroup1 = match.Groups[2].Value;
                                                        string matchgroup2 = match.Groups[3].Value;
                                                        intFlightTimeH = int.Parse(match.Groups[2].Value);
                                                        intFlightTimeM = int.Parse(match.Groups[3].Value);
                                                    }
                                                    else
                                                    {                                                        
                                                        intFlightTimeM = int.Parse(match.Groups[0].Value.Replace("m",""));
                                                    }
                                                    TEMP_DurationTime = new TimeSpan(0, intFlightTimeH, intFlightTimeM, 0);

                                                }
                                                if (TEMP_Aircraftcode != null && TEMP_DurationTime != TimeSpan.MinValue)
                                                {
                                                    // Aircraft code is gevonden, dit moet nu de vlucht tijd zijn. En dus de laatste waarde in de reeks.                                        
                                                    string TEMP_Airline = null;
                                                    TEMP_Airline = TEMP_FlightNumber.Substring(0, 2);
                                                    if (TEMP_FlightMonday == false && TEMP_FlightTuesday == false && TEMP_FlightWednesday == false && TEMP_FlightThursday == false && TEMP_FlightFriday == false && TEMP_FlightSaterday == false && TEMP_FlightSunday == false)
                                                    {
                                                        Console.WriteLine("Flight without days skipped...");
                                                    }
                                                    else { 
                                                        CIFLights.Add(new CIFLight
                                                        {
                                                            FromIATA = TEMP_FromIATA,
                                                            ToIATA = TEMP_ToIATA,
                                                            FromDate = TEMP_ValidFrom,
                                                            ToDate = TEMP_ValidTo,
                                                            ArrivalTime = TEMP_ArrivalTime,
                                                            DepartTime = TEMP_DepartTime,
                                                            FlightAircraft = TEMP_Aircraftcode,
                                                            FlightAirline = TEMP_Airline,
                                                            FlightMonday = TEMP_FlightMonday,
                                                            FlightTuesday = TEMP_FlightTuesday,
                                                            FlightWednesday = TEMP_FlightWednesday,
                                                            FlightThursday = TEMP_FlightThursday,
                                                            FlightFriday = TEMP_FlightFriday,
                                                            FlightSaterday = TEMP_FlightSaterday,
                                                            FlightSunday = TEMP_FlightSunday,
                                                            FlightNumber = TEMP_FlightNumber,
                                                            FlightOperator = TEMP_FlightOperator,
                                                            FlightDuration = TEMP_DurationTime.ToString().Replace("-", ""),
                                                            FlightCodeShare = TEMP_FlightCodeShare,
                                                            FlightNextDayArrival = TEMP_FlightNextDayArrival,
                                                            FlightNextDays = TEMP_FlightNextDays
                                                        });
                                                    }

                                                    // Cleaning All but From and To 
                                                    TEMP_ValidFrom = new DateTime();
                                                    TEMP_ValidTo = new DateTime();
                                                    TEMP_Conversie = 0;
                                                    TEMP_FlightMonday = false;
                                                    TEMP_FlightTuesday = false;
                                                    TEMP_FlightWednesday = false;
                                                    TEMP_FlightThursday = false;
                                                    TEMP_FlightFriday = false;
                                                    TEMP_FlightSaterday = false;
                                                    TEMP_FlightSunday = false;
                                                    TEMP_DepartTime = new DateTime();
                                                    TEMP_ArrivalTime = new DateTime();
                                                    TEMP_FlightNumber = null;
                                                    TEMP_Aircraftcode = null;
                                                    TEMP_DurationTime = TimeSpan.MinValue;
                                                    TEMP_FlightCodeShare = false;
                                                    TEMP_FlightNextDayArrival = false;
                                                    TEMP_FlightNextDays = 0;
                                                    TEMP_FlightOperator = null;
                                                }
                                                dayset = dayset + 1;
                                            }
                                        }
                                    }  
                                }
                            }
                        }

                    }


                    //text.Append(currentText);                    
                }
            }

            // You'll do something else with it, here I write it to a console window
            // Console.WriteLine(text.ToString());

            // Write the list of objects to a file.
            System.Xml.Serialization.XmlSerializer writer =
            new System.Xml.Serialization.XmlSerializer(CIFLights.GetType());
            string myDir = AppDomain.CurrentDomain.BaseDirectory + "\\output";
            System.IO.Directory.CreateDirectory(myDir);
            System.IO.StreamWriter file =
               new System.IO.StreamWriter("output\\output.xml");

            writer.Serialize(file, CIFLights);
            file.Close();

            //Console.ReadKey();
            Console.WriteLine("Insert into Database...");
            for (int i = 0; i < CIFLights.Count; i++) // Loop through List with for)
            {
                using (SqlConnection connection = new SqlConnection("Server=(local);Database=CI-Import;Trusted_Connection=True;"))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;            // <== lacking
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "InsertFlight";
                        command.Parameters.Add(new SqlParameter("@FlightSource", "United"));
                        command.Parameters.Add(new SqlParameter("@FromIATA", CIFLights[i].FromIATA));
                        command.Parameters.Add(new SqlParameter("@ToIATA", CIFLights[i].ToIATA));
                        command.Parameters.Add(new SqlParameter("@FromDate", CIFLights[i].FromDate));
                        command.Parameters.Add(new SqlParameter("@ToDate", CIFLights[i].ToDate));
                        command.Parameters.Add(new SqlParameter("@FlightMonday", CIFLights[i].FlightMonday));
                        command.Parameters.Add(new SqlParameter("@FlightTuesday", CIFLights[i].FlightTuesday));
                        command.Parameters.Add(new SqlParameter("@FlightWednesday", CIFLights[i].FlightWednesday));
                        command.Parameters.Add(new SqlParameter("@FlightThursday", CIFLights[i].FlightThursday));
                        command.Parameters.Add(new SqlParameter("@FlightFriday", CIFLights[i].FlightFriday));
                        command.Parameters.Add(new SqlParameter("@FlightSaterday", CIFLights[i].FlightSaterday));
                        command.Parameters.Add(new SqlParameter("@FlightSunday", CIFLights[i].FlightSunday));
                        command.Parameters.Add(new SqlParameter("@DepartTime", CIFLights[i].DepartTime));
                        command.Parameters.Add(new SqlParameter("@ArrivalTime", CIFLights[i].ArrivalTime));
                        command.Parameters.Add(new SqlParameter("@FlightNumber", CIFLights[i].FlightNumber));
                        command.Parameters.Add(new SqlParameter("@FlightAirline", CIFLights[i].FlightAirline));
                        command.Parameters.Add(new SqlParameter("@FlightOperator", CIFLights[i].FlightOperator));
                        command.Parameters.Add(new SqlParameter("@FlightAircraft", CIFLights[i].FlightAircraft));
                        command.Parameters.Add(new SqlParameter("@FlightCodeShare", CIFLights[i].FlightCodeShare));
                        command.Parameters.Add(new SqlParameter("@FlightNextDayArrival", CIFLights[i].FlightNextDayArrival));
                        command.Parameters.Add(new SqlParameter("@FlightDuration", CIFLights[i].FlightDuration));
                        command.Parameters.Add(new SqlParameter("@FlightNextDays", CIFLights[i].FlightNextDays));
                        foreach (SqlParameter parameter in command.Parameters)
                        {
                            if (parameter.Value == null)
                            {
                                parameter.Value = DBNull.Value;
                            }
                        }


                        try
                        {
                            connection.Open();
                            int recordsAffected = command.ExecuteNonQuery();
                        }

                        finally
                        {
                            connection.Close();
                        }
                    }
                }

            }


        }




    }


}
