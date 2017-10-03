using CBIB.Models;
using System.Collections.Generic;

namespace CBIB.Views.ViewModels
{
    public class NodesAddRoleViewModel
    {
        public string UserId { get; set; }
        public string NewNode { get; set; }
        public List<Node> Nodes { get; set; }
        public string Email { get; set; }
    }
}
