using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.EntityClient;
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
    public class AssetDTO
    {
        public svCams.AssetId AssetID { get; set; }
        public string AssetType { get; set; }
        public string ActiveStatus { get; set; }

    }


}
