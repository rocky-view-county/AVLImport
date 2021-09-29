using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Security;
using System.Data.Entity.Spatial;
using ProjNet;
using ProjNet.CoordinateSystems;
using ProjNet.Converters.WellKnownText;
using ProjNet.CoordinateSystems.Transformations;


namespace ImportAVLData
{
    class Program
    {
        static void Main(string[] args)
        {
                       
            try
            {
                ////if (DateTime.Today.DayOfWeek == DayOfWeek.Sunday)                
                ////    Util.UpdateVehicleList();

                //DateTime startDate; DateTime endDate;
                //if (args.Count() == 2 && int.Parse ( args[1] ) > 3)
                //{
                //    startDate = DateTime.Now.Subtract(TimeSpan.FromDays( int.Parse(args[1]) ));
                //    endDate = DateTime.Now.Date;
                    
                //}
                //else
                //{

                //}                    

                //List<DateTime> a = Util.AllDatesBetween( startDate, DateTime.Now, 3).ToList();

                if (args.Length > 1)
                {
                    DateTime startProcess = DateTime.Now;
                    Console.Write("The process starts at " + startProcess.ToString() + "\n");
                    

                    Util.UpdateVehicleList();
                    
                    if (args[0].ToUpper() == "ALL")
                    {
                        string[] lstType = { "Grader", "Snowplow/Sander", "Public Works", "Public Works Semi", "Patch Truck", "Plow Sander Pickup", "Mower", "Sprayer", "Supervisor", "Water Truck", "Ag Pickup", "Transportation", "4x4" , "Limo SUV" };

                        foreach (string vtype in lstType)
                        {
                            args[0] = vtype;
                            Util.GetCamsVehiclePlots(Util.GetPlotRequests(args));
                        }
                    }
                    else
                    {
                        Util.GetCamsVehiclePlots(Util.GetPlotRequests(args));
                    }



                    
                    DateTime endProcess = DateTime.Now;

                    Console.Write("The Process is finished at " + endProcess.ToString() + "\n" 
                                + " It took " + endProcess.Subtract(startProcess).TotalSeconds + " seconds");

                }
                else
                {
                    Console.Write("Wrong paramenter input");
                    Console.ReadKey();
                }                
                

            }

            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());

                string fromMail = "cityworks@mdrockyview.ab.ca";
                string toMail = "cmoon@rockyview.ca";
                string mailSubject = "AVL Importing Process failed";
                string mailBody = "\n" + ex.ToString() + "\n";                
                Util.SendMail(fromMail, toMail, mailSubject,mailBody);
            }          


        }



    }
}
