using System.Collections.Generic;


namespace HomesteadAzureFunctionApp
{
    public static class MockDB
    {
        public static List<SeasonData> GetSeasonData()
        {
            List<SeasonData> seasonDatas = new List<SeasonData>();
            seasonDatas.Add(new SeasonData(1, "autumn", "tomatoes", "tomatoe.jpg", false, false, true));
            seasonDatas.Add(new SeasonData(2, "autumn", "broccoli", "broc.jpg", true, true, false));
            seasonDatas.Add(new SeasonData(3, "winter", "corn", "corn.jpg", true, true, false));

            return seasonDatas;
        }
    }

    public class SeasonData
    {
        public SeasonData(){ }
        public SeasonData(int id, string season, string crop, string url, bool sow, bool seeding, bool harvest, bool complete = false)
        {
            Season = season;
            Crop = crop;
            IconUrl = url;
            Sow = sow;
            Seeding = seeding;
            Harvest = harvest;
            Complete = complete;
            ID = id;
        }

       
        public int ID { get; set; }
        public string Season { get; set; }
        public string Crop { get; set; }
        public string IconUrl { get; set; }
        public bool Sow { get; set; }
        public bool Seeding { get; set; }
        public bool Harvest { get; set; }
        public bool Complete { get; set; }
    }

}


/*
 * public class GetNamesTest {
    enum Colors { Red, Green, Blue, Yellow };
    enum Styles { Plaid, Striped, Tartan, Corduroy };

    public static void Main() {

        Console.WriteLine("The members of the Colors enum are:");
        foreach(string s in Enum.GetNames(typeof(Colors)))
            Console.WriteLine(s);

        Console.WriteLine();

        Console.WriteLine("The members of the Styles enum are:");
        foreach(string s in Enum.GetNames(typeof(Styles)))
            Console.WriteLine(s);
    }
}
// The example displays the following output:
//       The members of the Colors enum are:
//       Red
//       Green
//       Blue
//       Yellow
//       
//       The members of the Styles enum are:
//       Plaid
//       Striped
//       Tartan
//       Corduroy

    */