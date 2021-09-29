using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using ProjNet.Converters.WellKnownText;
using ProjNet.CoordinateSystems.Transformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImportAVLData
{
    public class Vehicle
    {


        public int Seq { get; set; }
        public ushort Id { get; set; }
        public DateTime CollectedTime { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public uint State { get; set; }
        public double Speed { get; set; }
        

        private string wkt_WGS84 = "GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137,298.257223563]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.017453292519943295]]";
        private string pcs_3TM = "PROJCS[\"NAD83 / Alberta 3TM ref merid 114 W\", GEOGCS[\"GCS_North_American_1983\", DATUM[\"D_North_American_1983\", SPHEROID[\"GRS_1980\", 6378137, 298.257222101]], PRIMEM[\"Greenwich\", 0], UNIT[\"Degree\", 0.0174532925199433]], PROJECTION[\"Transverse_Mercator\"], PARAMETER[\"latitude_of_origin\", 0], PARAMETER[\"central_meridian\", -114], PARAMETER[\"scale_factor\", 0.9999], PARAMETER[\"false_easting\", 0], PARAMETER[\"false_northing\", 0], UNIT[\"Meter\", 1]]";


        public string SysWithDate
        {
            get
            {
                return  Id.ToString() + "_" +  CollectedTime.Year.ToString() + "_" + CollectedTime.Month.ToString() + "_" + CollectedTime.Day.ToString();
            }
        }

        private static double[] ConvertWGS84To3TM(double lat, double lon, string wkt_WGS84, string wkt_3deg)
        {

            try
            {
                ICoordinateSystem gcs_WGS84 = CoordinateSystemWktReader.Parse(wkt_WGS84) as ICoordinateSystem;

                IProjectedCoordinateSystem pcs_3TM = CoordinateSystemWktReader.Parse(wkt_3deg) as IProjectedCoordinateSystem;

                CoordinateTransformationFactory ctfac = new CoordinateTransformationFactory();

                ICoordinateTransformation trans = ctfac.CreateFromCoordinateSystems(gcs_WGS84, pcs_3TM);

                double[] fromPoint = new double[] { lon, lat };  // U2U Consult Head Office, in degrees

                double[] toPoint = trans.MathTransform.Transform(fromPoint);

                return toPoint;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
                return null;
            }


        }

        public double[] XYCoord
        {
            get
            {
                return
                ConvertWGS84To3TM(this.Lat, this.Long, this.wkt_WGS84, this.pcs_3TM);
            }
        }

        public double XCoord
        {
            get
            {
                if (XYCoord != null)
                    return XYCoord[0];
                else
                    return 0;

            }

        }

        public double YCoord
        {
            get
            {
                if (XYCoord != null)
                    return XYCoord[1];
                else
                    return 0;

            }

        }

        public string VehicleType
        {
            get
            {
                return String.IsNullOrEmpty(Util.GetVehicleType(Id)) ? string.Empty : Util.GetVehicleType(Id);


            }
        }

        public string StateDesc
        {
            get
            {
                if (VehicleType != "Grader")
                {

                    if (State == 0)
                        return "No Event";
                    else
                        return String.IsNullOrEmpty(Util.GetVehicleStateDesc(Id, (int)State, VehicleType)) ? string.Empty : Util.GetVehicleStateDesc(Id, (int)State, VehicleType);
                }
                else
                {
                    return Util.GetGraderStateDesc(Speed);
                }
            }
        }



    }
}
