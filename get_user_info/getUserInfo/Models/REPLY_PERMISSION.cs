//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class REPLY_PERMISSION
    {
        public byte SYSTEM_CODE { get; set; }
        public byte RETURN_ITEM_CODE { get; set; }
        public bool DELETE_FLAG { get; set; }
        public string DELETE_HOST_NAME { get; set; }
        public Nullable<System.DateTime> DELETE_DATE_TIME { get; set; }
    
        public virtual RETURN_ITEM RETURN_ITEM { get; set; }
        public virtual SYSTEM_INFORMATION SYSTEM_INFORMATION { get; set; }
    }
}
