//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FunskoolApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class orderDetail
    {
        public int OrderDetail1 { get; set; }
        public int toyid { get; set; }
        public int OrderId { get; set; }
    
        public virtual Order Order { get; set; }
        public virtual Toy Toy { get; set; }
    }
}