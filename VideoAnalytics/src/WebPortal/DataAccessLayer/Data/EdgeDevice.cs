namespace DataAccessLayer.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EdgeDevice")]
    public partial class EdgeDevice
    {
        public int Id { get; set; }

        [StringLength(63)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(50)]
        public string OSType { get; set; }

        public int? Capacity { get; set; }

        [StringLength(511)]
        public string ConnectString { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Configurations { get; set; }

    }

    public enum EdgeDeviceStatus
    {
        Create,
        Disconnected,
        Connected
    };

}
