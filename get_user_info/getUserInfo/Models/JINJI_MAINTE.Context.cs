﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはテンプレートから生成されました。
//
//     このファイルを手動で変更すると、アプリケーションで予期しない動作が発生する可能性があります。
//     このファイルに対する手動の変更は、コードが再生成されると上書きされます。
// </auto-generated>
//------------------------------------------------------------------------------

namespace getUserInfo.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class JINJI_MAINTEEntities2 : DbContext
    {
        public JINJI_MAINTEEntities2()
            : base("name=JINJI_MAINTEEntities2")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<REPLY_PERMISSION> REPLY_PERMISSION { get; set; }
        public virtual DbSet<RETURN_ITEM> RETURN_ITEM { get; set; }
        public virtual DbSet<SYSTEM_INFORMATION> SYSTEM_INFORMATION { get; set; }
    }
}
