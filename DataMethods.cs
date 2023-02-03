using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;
using System.Web.Mvc;
using System.Net.Http;
using Microsoft.AspNetCore.Http;


namespace IndigoLabsNaloga
{
    public class DataMethods
    {



        public static List<ResultSet> GetCases(string region, string from, string to)
        {
            //check if region is one of validValues
            string[] validValues = { "LJ", "CE", "KR", "NM", "KK", "KP", "MB", "MS", "NG", "PO", "SG", "ZA" };

            if (Array.IndexOf(validValues, region) != -1)
            {
                Console.WriteLine("The input value is valid.");
                DateTime datef;
                if (!DateTime.TryParse(from, out datef)) 
                {
                    throw new Exception("The date is not in the correct format.");
                }
                DateTime datet;
                if (!DateTime.TryParse(to, out datet))
                {
                    throw new Exception("The date is not in the correct format.");
                }

                //converted date to right format
                string[] f = from.Split(".");
                DateOnly dateFrom = new DateOnly(int.Parse(f[2]), int.Parse(f[1]), int.Parse(f[0]));

                string[] t = to.Split(".");
                DateOnly dateTo = new DateOnly(int.Parse(t[2]), int.Parse(t[1]), int.Parse(t[0]));

                //check if from and to are in right date format
                if (dateFrom.ToString("dd.MM.yyyy") == dateFrom.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture) && dateTo.ToString("dd.MM.yyyy") == dateTo.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture))
                {
                    

                    var results = new List<ResultSet>();

                    var webClient = new WebClient();
                    var csvData = webClient.DownloadString("https://raw.githubusercontent.com/sledilnik/data/master/csv/region-cases.csv");

                    //get first line, save it to array to get column names and their position 
                    string[] firstLineValues;

                    using (var reader = new StringReader(csvData))
                    {
                        string firstLine = reader.ReadLine();
                        firstLineValues = firstLine.Split(",");

                        //setting the positions of right columns
                        String date = firstLineValues[0];
                        String reg = region.ToLower();

                        String a = "region." + reg + ".cases.active";
                        int activePosition = Array.IndexOf(firstLineValues, a);


                        String v1 = "region." + reg + ".vaccinated.1st.todate";
                        int v1Postition = Array.IndexOf(firstLineValues, v1);


                        String v2 = "region." + reg + ".vaccinated.2nd.todate";
                        int v2Postition = Array.IndexOf(firstLineValues, v2);


                        String d = "region." + reg + ".deceased.todate";
                        int deceasedPosition = Array.IndexOf(firstLineValues, d);

                        //go throught the other lines and for every line create new ResultSet, set values, save in results. 
                        //while reader.readline != null
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var parts = line.Split(",");
                            ResultSet result = new ResultSet();
                            DateOnly dateOnly = DateOnly.Parse(parts[0]);

                            //if date is between from and to
                            if (dateOnly.CompareTo(dateFrom) >= 0 && dateOnly.CompareTo(dateTo) <= 0)
                            {
                                result.Date = parts[0];
                                result.Region = region;

                                if (parts[activePosition] == "")
                                {
                                    result.ActiveCases = -1;
                                }
                                else result.ActiveCases = int.Parse(parts[activePosition]);

                                if (parts[v1Postition] == "")
                                {
                                    result.Vaccinated1st = -1;

                                }
                                else result.Vaccinated1st = int.Parse(parts[v1Postition]);

                                if (parts[v2Postition] == "")
                                {
                                    result.Vaccinated2nd = -1;
                                }
                                else result.Vaccinated2nd = int.Parse(parts[v2Postition]);

                                if (parts[deceasedPosition] == "")
                                {
                                    result.Deceased = -1;
                                }
                                else result.Deceased = int.Parse(parts[deceasedPosition]);


                                results.Add(result);
                            }

                        }
                    }


                    return results;
                }
                else
                {
                    
                    throw new Exception("The date is not in the correct format.");
                }
            }
            else
            {
                
                throw new Exception("The input value is not valid.");
            }
  
        }
    


        //public static List<ResultSet> GetData(DateOnly from, DateOnly to, string region)
        
        //{

        //    var results = new List<ResultSet>();

        //    var webClient = new WebClient();
        //    var csvData = webClient.DownloadString("https://raw.githubusercontent.com/sledilnik/data/master/csv/region-cases.csv");

        //    //get first line, save it to array to get column names and their position 
        //    string[] firstLineValues;

        //    using (var reader = new StringReader(csvData))
        //    { 
        //        string firstLine = reader.ReadLine();
        //        firstLineValues = firstLine.Split(",");

        //        //setting the positions of right columns
        //        String date = firstLineValues[0];
        //        String reg = region.ToLower();

        //        String a = "region." + reg + ".cases.active";
        //        int activePosition = Array.IndexOf(firstLineValues, a);


        //        String v1 = "region." + reg + ".vaccinated.1st.todate";
        //        int v1Postition = Array.IndexOf(firstLineValues, v1);


        //        String v2 = "region." + reg + ".vaccinated.2nd.todate";
        //        int v2Postition = Array.IndexOf(firstLineValues, v2);


        //        String d = "region." + reg + ".deceased.todate";
        //        int deceasedPosition = Array.IndexOf(firstLineValues, d);

        //        //go throught the other lines and for every line create new ResultSet, set values, save in results. 
        //        //while reader.readline != null
        //        string line;
        //        while ((line = reader.ReadLine()) != null)
        //        {
        //            var parts = line.Split(",");
        //            ResultSet result = new ResultSet();
        //            DateOnly dateOnly = DateOnly.Parse(parts[0]);

        //            //if date is between from and to
        //            if (dateOnly.CompareTo(from) >= 0 && dateOnly.CompareTo(to) <= 0) 
        //            {
        //                result.Date = parts[0];
        //                result.Region = region;

        //                if (parts[activePosition] == "")
        //                {
        //                    result.ActiveCases = -1;
        //                }
        //                else result.ActiveCases = int.Parse(parts[activePosition]);

        //                if (parts[v1Postition] == "")
        //                {
        //                    result.Vaccinated1st = -1;

        //                }
        //                else result.Vaccinated1st = int.Parse(parts[v1Postition]);

        //                if (parts[v2Postition] == "")
        //                {
        //                    result.Vaccinated2nd = -1;
        //                }
        //                else result.Vaccinated2nd = int.Parse(parts[v2Postition]);

        //                if (parts[deceasedPosition] == "")
        //                {
        //                    result.Deceased = -1;
        //                }
        //                else result.Deceased = int.Parse(parts[deceasedPosition]);


        //                results.Add(result);
        //            }

        //        }
        //    }


        //    return results;

        //}

        public static List<LastWeekSet> GetLastWeekSets()
        {

            var results = new List<LastWeekSet>();

            var webClient = new WebClient();
            var csvData = webClient.DownloadString("https://raw.githubusercontent.com/sledilnik/data/master/csv/region-cases.csv");

            //for each region get column position of active cases in that region

            //get first line, save it to array to get column names and their position 
            string[] firstLineValues;
            using (var reader = new StringReader(csvData))
            {
                string firstLine = reader.ReadLine();
                firstLineValues = firstLine.Split(",");

                //setting the positions of right columns
                //String date = firstLineValues[0];

                //all regions
                string[] regions = { "lj", "ce", "kr", "nm", "kk", "kp", "mb", "ms", "ng", "po", "sg", "za" };

                //dictionary to store values inside;
                IDictionary<string, int> dict = new Dictionary<string, int>() {
                    {"lj",0 }, {"ce",0 },{"kr",0 },{"nm",0 },{"kk",0 },{"kp",0 },
                    {"mb",0 },{"ms",0 },{"ng",0 },{"po",0 },{"sg",0 },{"za",0 }
                };

                //get right format for current date and set max date(+7 days)
                String test = DateTime.Now.ToString("dd.MM.yyy");
                DateOnly currentDate = DateOnly.Parse(test);
                DateOnly maxDate = currentDate.AddDays(-7);


                //for each date
                string line;
                int numCases = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    //get current column with date
                    var splited = line.Split(",");
                    DateOnly currentColumnDate = DateOnly.Parse(splited[0]);
                    if (currentColumnDate >= maxDate)
                    {
                        foreach (string region in regions)
                        {

                            if (region == "ce") 
                            {
                                Console.WriteLine(region);     
                            }
                            String regionActive = "region." + region + ".cases.active";
                            numCases = 0;
                            //value of column with active cases in region
                            int activePosition = Array.IndexOf(firstLineValues, regionActive);
                            // current value of key
                            int tmp = dict[region];
                            
                            numCases = int.Parse(splited[activePosition]) + tmp;

                            //write new value of cases
                            dict[region] = numCases;

                          

                        }
                        
                    }


                }

                //devid all active cases by 7 to get average for each region
                foreach (KeyValuePair<string, int> entry in dict)
                {
                    LastWeekSet lastWeekSet = new LastWeekSet();
                    lastWeekSet.Region = entry.Key;
                    lastWeekSet.AverageCases = entry.Value / 7;
                    results.Add(lastWeekSet);

                }

                return results;

            }

        }
    }
}
