#region Usings

using System.ComponentModel.DataAnnotations;

#endregion

namespace Terministrator.Terministrator.Entites
{
    /// <summary>
    /// Entity of the files. Contains all the datas required for a file sent by a user on an application.
    /// </summary>
    class File : MessageContent
    {
        public string FileName { get; set; }
    }
}