//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DatabaseLayer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class DoctorAppointTable
    {
        public int DoctorAppointID { get; set; }
        [Required(ErrorMessage = "*Required!")]
        public int DoctorID { get; set; }
        [Required(ErrorMessage = "*Required!")]
        public int PatientID { get; set; }
        [Required(ErrorMessage = "*Required!")]
        public int DoctorTimeSlotID { get; set; }
        [Required(ErrorMessage = "*Required!")]
        [DataType(DataType.Date)]
        public System.DateTime AppointDate { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "*Required!")]
        public bool IsFeeSubmit { get; set; }
        public bool IsChecked { get; set; }
        [Required(ErrorMessage = "*Required!")]
        public string TransectionNo { get; set; }
        public string DoctorComment { get; set; }
    
        public virtual DoctorTable DoctorTable { get; set; }
        public virtual DoctorTimeSlotTable DoctorTimeSlotTable { get; set; }
        public virtual PatientTable PatientTable { get; set; }
    }
}
