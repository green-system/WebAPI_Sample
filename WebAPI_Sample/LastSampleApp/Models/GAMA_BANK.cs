//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはテンプレートから生成されました。
//
//     このファイルを手動で変更すると、アプリケーションで予期しない動作が発生する可能性があります。
//     このファイルに対する手動の変更は、コードが再生成されると上書きされます。
// </auto-generated>
//------------------------------------------------------------------------------

namespace LastSampleApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class GAMA_BANK
    {
        public string BANK_CD { get; set; }
        public string OFFICE_CD { get; set; }
        public byte OFFICE_FLG { get; set; }
        public string BANK_OR_OFFICE_NAME { get; set; }
        public string BANK_OR_OFFICE_KANA { get; set; }
        public byte DELETE_FLG { get; set; }
        public Nullable<System.DateTime> MANUAL_UPDATE { get; set; }
        public Nullable<byte> ADMIN_NUMBER { get; set; }
        public Nullable<System.DateTime> SYSTEM_UPDATE { get; set; }
    }
}
