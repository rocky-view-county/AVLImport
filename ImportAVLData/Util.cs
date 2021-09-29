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
using System.Net.Mail;
using System.Collections;



namespace ImportAVLData
{
    public class Util
    {
        public static IEnumerable<DateTime> AllDatesBetween(DateTime start, DateTime end, int span)
        {
            for (var day = start.Date; day <= end; day = day.AddDays(span))
                yield return day;
        }

        public static void GetCamsVehiclePlots(svCams.PlotPointRequest[] vds)
        {
            try
            {

                if (vds.Count() > 0)
                {
                    #region //Process Vehicles

                    #region //Initialize Cams API
                    //////////////////////////////////////////////////////////////////////////////////////////////////
                    svCams.CAMSDataInterfaceClient _proxy = new svCams.CAMSDataInterfaceClient();
                    // Set the user’s credentials on the proxy
                    _proxy.ClientCredentials.UserName.UserName = "CAMSAdmin";
                    _proxy.ClientCredentials.UserName.Password = "gemini";
                    // Treat the test certificate as trusted
                    _proxy.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
                    ////////////////////////////////////////////////////////////////////////////////////////////////////
                    var allPoints = _proxy.GetPlotData(vds).PlotPoints;
                    
                    #endregion                   

                    string sVehicleType = string.Empty;
                    foreach (svCams.PlotPointRequest vd in vds)
                    {
                        //Search by SysId and GMT Time. Be careful. It is not Local Time. GMT Time.
                        var hists = allPoints.Where(k => k.Key.Id == vd.EAId.Id)
                                    .Select(s => s.Value).First()
                                    .OrderBy(k => k.TimeGMT)// Sort by CollectedTime
                                    .GroupBy(x => new { x.TimeGMT.Date, x.Lat,x.Lng }) // Remove Duplicate Points with the same X, Y Coordinate                                     
                                    .Select((s, index) => new Vehicle
                                    {
                                        Seq = index,
                                        Id = (ushort)vd.EAId.Assetid.Id,
                                        CollectedTime = s.First().TimeGMT.ToLocalTime(),
                                        Lat = s.First().Lat,
                                        Long = s.First().Lng,
                                        Speed = s.First().Speed,
                                        State = (uint)s.First().State
                                    });



                        if (hists.Count() > 100)
                        {
                            sVehicleType = hists.First().VehicleType;
                            DateTime endDate = vd.EndGMT.ToLocalTime();

                            Console.Write("\n Vehicle Type:" + sVehicleType + " Sysid:" + vd.EAId.Assetid.Id.ToString() +
                                " process started. \n The dates when points captured are between " + vd.StartGMT.ToLocalTime().ToShortDateString() + 
                                " and " + vd.EndGMT.AddDays(-1).ToShortDateString() + "\n");

                            Console.Write(" " + hists.Count().ToString() + " points are captured. \n");
                            
                            #region // Vehicle List
                            var entities = from v1 in hists
                                           join v2 in hists on new { seq = v1.Seq, dateSys = v1.SysWithDate }
                                                               equals new { seq = v2.Seq - 1, dateSys = v2.SysWithDate }
                                           select new ActiveA
                                           {
                                               Lat = v1.Lat,
                                               Lon = v1.Long,
                                               SeqOrder = v1.Seq,
                                               CollectedTime = v1.CollectedTime,
                                               Shape = Util.CreateGeomPoint(v1.XYCoord),
                                               ShapeGeography = Util.CreatePoint(v1.Lat, v1.Long, 4326),
                                               ShapeLine = Util.CreateGeomLine(v1.XYCoord, v2.XYCoord),
                                               SysId = vd.EAId.Assetid.Id,
                                               X_Coord = v1.XCoord,
                                               Y_Coord = v1.YCoord,
                                               State = (ushort)v1.State,
                                               StateDesc = v1.StateDesc,
                                               Speed = v1.Speed,
                                               SysWithDate = v1.SysWithDate,
                                               SecDiff = (int)v2.CollectedTime.Subtract(v1.CollectedTime).TotalSeconds,
                                               VehicleType = v1.VehicleType,
                                               StreetView = "http://gismo.mdrockyview.ab.ca/ParcelInformationSystem/StreetView.htm?lat="
                                                           + v1.Lat.ToString() + " & lng =" + v1.Long.ToString()

                                           };



                            #endregion  //

                            #region //Vehicle Aggregated List
                            var agg = entities.GroupBy(s => new { s.SysWithDate, s.StateDesc })
                                                    .Select(s => new ActiveB
                                                    {
                                                        StateDesc = s.First().StateDesc,
                                                        SysWithDate = s.First().SysWithDate,
                                                        SysID = s.First().SysId,
                                                        ActivityTime = s.First().CollectedTime.Value.Date,
                                                        ShapeAgg = s.Select(k => k.Shape).Aggregate((b, c) => b.Union(c)),
                                                        ShapeLineAgg = s.Select(k => k.ShapeLine).Aggregate((b, c) => b.Union(c)),
                                                        SumOfSec = s.Sum(k => k.SecDiff),
                                                        VehicleType = s.First().VehicleType,
                                                        Shape = s.Select(k => k.ShapeLine).Aggregate((b, c) => b.Union(c)).Buffer(0.25),
                                                        SumOfDistance = (double)Math.Round(

                                                            (decimal)s.Select(k => k.ShapeLine).Aggregate((b, c) => b.Union(c)).Length / 1000, 2


                                                            )


                                                    });

                            #endregion

                            Util1.UpdateVehicleListPlot(entities, agg, vd, sVehicleType);

                        }

                    }
                    #endregion

                }
                else
                {
                    Console.Write("There is no vehicle list. Please check your parameter input");
                }



            }

            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                Console.ReadKey();

                string fromMail = "cityworks@mdrockyview.ab.ca";
                string toMail = "cmoon@rockyview.ca";
                string mailSubject = "AVL Importing Process failed";
                string mailBody = "\n" + ex.ToString() + "\n";
                Util.SendMail(fromMail, toMail, mailSubject, mailBody);
            }


        }

        public static svCams.PlotPointRequest[] GetPlotRequests(string[] args)
        {
            try
            {                               
                //Get Start and End Date
                string[] dayArgs = new string[args.Length - 1];
                Array.Copy(args, 1, dayArgs, 0, args.Length - 1);
                DateTime[] startEndDate = GetStartEndDate(dayArgs);

                //Intitialize PlotPointRequest
                svCams.PlotPointRequest[] requestList;

                int sysId = 0;

                //When only single sysId is input.
                if (int.TryParse(args[0], out sysId))
                {   //Instanciate one request
                    svCams.PlotPointRequest request =

                    new svCams.PlotPointRequest
                    {
                        EAId = new svCams.EventAssetId
                        {
                            Assetid = new svCams.AssetId { Id = (ushort)sysId, ServerName = "CAMSAdmin" },
                            Id = (ushort)sysId
                        },
                        //Convert local time to UT Time
                        StartGMT = startEndDate[0].ToUniversalTime(),
                        EndGMT = startEndDate[1].ToUniversalTime()
                    };

                    requestList = new svCams.PlotPointRequest[1];
                    requestList[0] = request;

                }
                //When Vehicle Type is input
                else
                {

                    string AssetType = args[0];
                    //Convert GMT Time
                    DateTime startDate = startEndDate[0];
                    DateTime endDate = startEndDate[1];
                    using (dcAVL dc = new dcAVL())
                    {
                        dc.Database.CommandTimeout = 7200;

                        if (AssetType.ToUpper() == "ALL")
                        {
                            var VehicleList = dc.VehicleLists.Where(s => s.Status == "Active").ToList();

                            requestList = VehicleList.Select(s => new svCams.PlotPointRequest
                            {

                                EAId = new svCams.EventAssetId
                                {
                                    Assetid = new svCams.AssetId { Id = (ushort)s.SysId, ServerName = "CAMSAdmin" },
                                    Id = (ushort)s.SysId
                                },

                                StartGMT = startEndDate[0].ToUniversalTime(),
                                EndGMT = startEndDate[1].ToUniversalTime()
                            }).ToArray();


                        }
                        else
                        {
                            var VehicleList = dc.VehicleLists.Where(s => s.VehicleType == AssetType && s.Status == "Active").ToList();

                            requestList = VehicleList.Select(s => new svCams.PlotPointRequest
                            {

                                EAId = new svCams.EventAssetId
                                {
                                    Assetid = new svCams.AssetId { Id = (ushort)s.SysId, ServerName = "CAMSAdmin" },
                                    Id = (ushort)s.SysId
                                },

                                StartGMT = startEndDate[0].ToUniversalTime(),
                                EndGMT = startEndDate[1].ToUniversalTime()
                            }).ToArray();

                        }

                    }

                }

                return requestList;

            }

            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                string fromMail = "cityworks@mdrockyview.ab.ca";
                string toMail = "cmoon@rockyview.ca";
                string mailSubject = "AVL Importing Process failed";
                string mailBody = "\n" + ex.ToString() + "\n";
                Util.SendMail(fromMail, toMail, mailSubject, mailBody);
                return null;
            }




        }

       //Get Start and End date
        public static DateTime[] GetStartEndDate(string[] dayArgs)
        {   // This function return local time. Not UTC Time
            try
            {

                int dayBack = 0;
                DateTime[] StartEndDate = { DateTime.Today.AddDays(-1) , DateTime.Today };

                #region // Date parameter calculation
                //When Parameter two, the second parameter is used to calculate start and end date.
                if (dayArgs.Count() == 1)
                {                 
                    
                    if (int.TryParse(dayArgs[0],out dayBack))
                    {
                        dayBack = -dayBack;
                        
                    }
                    else
                    {
                        Console.WriteLine("Please input number to go back to start date");
                    }
                    
                    //Convert GMT to Local Time
                    StartEndDate[0] = DateTime.Today.AddDays(dayBack).Date;
                    StartEndDate[1] = DateTime.Now.Date.AddDays(1).Date;

                }
                else if (dayArgs.Count() == 2)
                {

                    if (DateTime.TryParse(dayArgs[0].ToString(), out StartEndDate[0]))
                        Console.WriteLine("Start Date is " + dayArgs[0]);
                    else
                    {
                        Console.WriteLine("Please input right datetime format for start date");
                    }

                    if (DateTime.TryParse(dayArgs[1].ToString(), out StartEndDate[1]))
                    {
                        Console.WriteLine("End Date is " + dayArgs[1]);
                        StartEndDate[1] = StartEndDate[1].AddDays(1);
                    }

                    else
                    {
                        Console.WriteLine("Please input right datetime format for end date");

                    }

                }

                return StartEndDate;
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
                //Console.ReadKey();
                
                string fromMail = "cityworks@mdrockyview.ab.ca";
                string toMail = "cmoon@rockyview.ca";
                string mailSubject = "AVL Importing Process failed";
                string mailBody = "\n" + ex.ToString() + "\n";
                Util.SendMail(fromMail, toMail, mailSubject, mailBody);

                return null;
            }
                

            #endregion
        }                       
                         
        public static string GetVehicleType(int sysId)
        {
            using (dcAVL dc = new dcAVL())
            {
                if (dc.VehicleLists.Where(s => s.SysId == sysId).Count() > 0)
                    return dc.VehicleLists.Where(s => s.SysId == sysId).First().VehicleType;

                else
                    return string.Empty;

            }
        }

        public static string GetVehicleStateDesc(int sysId, int state, string vehicleType)
        {
            using (dcAVL dc = new dcAVL())
            {


                if (dc.StateLists.Where(s => s.SysId == sysId && s.State == state).Count() > 0)
                    return dc.StateLists.Where(s => s.SysId == sysId && s.State == state).First().StateDesc;
                else
                    return string.Empty;


            }
        }

        public static string GetGraderStateDesc(double speed)
        {
            using (dcAVL dc = new dcAVL())
            {
                if (dc.GraderStateLookUps.Where(s => s.SpeedMin <= speed && speed < s.SpeedMax).Count() > 0)
                    return dc.GraderStateLookUps.Where(s => s.SpeedMin <= speed && speed < s.SpeedMax).First().Dsr;
                else
                    return "No Event";
            }
        }
        
        public static DbGeography CreatePoint(double latitude, double longitude, int srid)
        {   
            //http://www.entityframeworktutorial.net/EntityFramework5/spatial-datatype-in-entity-framework5.aspx
            var text = string.Format("POINT({0} {1})", longitude, latitude);
            // 4326 is most common coordinate system used by GPS/Maps
            return DbGeography.PointFromText(text, srid);
        }

        public static DbGeometry CreateGeomPoint(double[] point)
        {
            var text = string.Format("POINT({0} {1})", point[0], point[1]);

            return DbGeometry.PointFromText(text, 3776);

        }

        public static DbGeometry CreateGeomLine(double[] fromPoint, double[] toPoint)
        {
            try
            {
                                
                string sLine = string.Format("LINESTRING ({0} {1} , {2} {3})", fromPoint[0], fromPoint[1], toPoint[0], toPoint[1]);

                return DbGeometry.LineFromText(sLine, 3776);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());

                string sLine = string.Format("LINESTRING ({0} {1} , {2} {3})", fromPoint[0], fromPoint[1], toPoint[0] + 1, toPoint[1] + 1);
                return DbGeometry.LineFromText(sLine, 3776);
            }


        }

        public static void SendMail(string from, string to, string subject, string body)
        {
            MailMessage mail = new MailMessage(from, to);

            SmtpClient client = new SmtpClient();
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "mail.mdrockyview.ab.ca";
            mail.Subject = subject;
            mail.Body = body;
            client.Send(mail);
        }

        //Update Vehicle List every Sunday
        public static void UpdateVehicleList()
        {
            try
            {

                svCams.CAMSDataInterfaceClient _proxy = new svCams.CAMSDataInterfaceClient();
                // Set the user’s credentials on the proxy
                _proxy.ClientCredentials.UserName.UserName = "CAMSAdmin";
                _proxy.ClientCredentials.UserName.Password = "gemini";
                // Treat the test certificate as trusted
                _proxy.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;

                var vehicleList = _proxy.GetMaintenanceAssets().Assets                                                              
                                                               .Select(
                                                                    s => new AssetDTO
                                                                    {
                                                                        AssetID = s.AInfo.Assetid
                                                                        ,
                                                                        AssetType = s.AInfo.AssetType
                                                                        ,
                                                                        ActiveStatus = s.ActiveState.ToString()
                                                                        
                                                                    }
                                                                  );


                //state
                var ids = vehicleList.Select(s => s.AssetID);

                var States = _proxy.GetAssetStates(ids.ToArray()).AssetStates;


                using (dcAVL dc = new dcAVL())
                {
                    //http://stackoverflow.com/questions/6232633/entity-framework-timeouts
                    dc.Database.CommandTimeout = 7200;

                    //http://stackoverflow.com/questions/15220411/entity-framework-delete-all-rows-in-table
                    dc.Database.ExecuteSqlCommand("Truncate Table AVL.dbo.VehicleList");
                    dc.Database.ExecuteSqlCommand("Truncate Table AVL.dbo.StateList");

                    foreach (AssetDTO asset in vehicleList)
                    {
                        dc.spUpdateVehicleList((int?)asset.AssetID.Id, asset.AssetType, asset.ActiveStatus);
                    }



                    foreach (var state in States)
                    {
                        foreach (var v in state.Value)
                        {
                            string assetType = vehicleList.Where(s => s.AssetID.Id == state.Key.Id).First().AssetType;

                            if (assetType == "Mower")
                                Console.Write(v.State.ToString() + " , " + v.StateDscr);

                            dc.spUpdateStateList((int?)state.Key.Id, assetType, (int?)v.State, v.StateDscr);

                        }
                    }
                }
            
            
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
                string fromMail = "cityworks@mdrockyview.ab.ca";
                string toMail = "cmoon@rockyview.ca";
                string mailSubject = "AVL Importing Process failed";
                string mailBody = "\n" + ex.ToString() + "\n";
                Util.SendMail(fromMail, toMail, mailSubject, mailBody);
                //Console.ReadKey();
            }

        }
        



        
    }
}
