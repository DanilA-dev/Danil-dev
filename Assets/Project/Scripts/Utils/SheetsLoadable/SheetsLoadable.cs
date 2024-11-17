using System;

namespace SheetsLoadable
{
    public class SheetsLoadable : Attribute
    {
        public string ValueName { get; set; }
        public int Сolumn { get; set; }
        
        public SheetsLoadable(string valueName, int сolumn = 1)
        {
            ValueName = valueName;
            Сolumn = сolumn;
        }
    }
    
   
        
}