//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Restaurant.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Shedule
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public int TableId { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<bool> Accepted { get; set; }
    
        public virtual Table Table { get; set; }
    }
}
