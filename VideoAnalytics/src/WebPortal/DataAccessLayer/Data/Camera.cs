namespace DataAccessLayer.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Camera")]
    public partial class Camera
    {
        public int Id { get; set; }

        [StringLength(63)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(1023)]
        public string Stream { get; set; }

        [StringLength(50)]
        public string Pipeline { get; set; }

        public int HostingDevice { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int FPS { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
