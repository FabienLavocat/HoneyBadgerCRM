using System.Collections.Generic;

namespace DnB.WindowsPhone.Core
{
    public class State
    {
        public string Short { get; set; }
        public string Name { get; set; }

        public string DisplayName
        {
            get { return string.Concat(Short, " - ", Name); }
        }

        public static IEnumerable<State> GetStates()
        {
            return new List<State>
                {
                    new State {Short = "AK", Name = "Alaska"},
                    new State {Short = "AL", Name = "Alabama"},
                    new State {Short = "AR", Name = "Arkansas"},
                    new State {Short = "AZ", Name = "Arizona"},
                    new State {Short = "CA", Name = "California"},
                    new State {Short = "CO", Name = "Colorado"},
                    new State {Short = "CT", Name = "Connecticut"},
                    new State {Short = "DC", Name = "Washington DC"},
                    new State {Short = "DE", Name = "Delaware"},
                    new State {Short = "FL", Name = "Florida"},
                    new State {Short = "GA", Name = "Georgia"},
                    new State {Short = "HI", Name = "Hawaii"},
                    new State {Short = "IA", Name = "Iowa"},
                    new State {Short = "ID", Name = "Idaho"},
                    new State {Short = "IL", Name = "Illinois"},
                    new State {Short = "IN", Name = "Indiana"},
                    new State {Short = "KS", Name = "Kansas"},
                    new State {Short = "KY", Name = "Kentuky"},
                    new State {Short = "LA", Name = "Louisiana"},
                    new State {Short = "MA", Name = "Massachusetts"},
                    new State {Short = "MD", Name = "Maryland"},
                    new State {Short = "ME", Name = "Maine"},
                    new State {Short = "MI", Name = "Michigan"},
                    new State {Short = "MN", Name = "Minnesota"},
                    new State {Short = "MO", Name = "Missouri"},
                    new State {Short = "MS", Name = "Mississippi"},
                    new State {Short = "MT", Name = "Montana"},
                    new State {Short = "NC", Name = "North Carolina"},
                    new State {Short = "ND", Name = "North Dakota"},
                    new State {Short = "NE", Name = "Nebraska"},
                    new State {Short = "NH", Name = "New Hampshire"},
                    new State {Short = "NJ", Name = "New Jersey"},
                    new State {Short = "NM", Name = "New Mexico"},
                    new State {Short = "NV", Name = "Nevada"},
                    new State {Short = "NY", Name = "New York"},
                    new State {Short = "OH", Name = "Ohio"},
                    new State {Short = "OK", Name = "Oklahoma"},
                    new State {Short = "OR", Name = "Oregon"},
                    new State {Short = "PA", Name = "Pennsylvania"},
                    new State {Short = "PR", Name = "Puerto Rico"},
                    new State {Short = "RI", Name = "Rhode Island"},
                    new State {Short = "SC", Name = "South Carolina"},
                    new State {Short = "SD", Name = "South Dakota"},
                    new State {Short = "TN", Name = "Tennessee"},
                    new State {Short = "TX", Name = "Texas"},
                    new State {Short = "UT", Name = "Utah"},
                    new State {Short = "VA", Name = "Virginia"},
                    new State {Short = "VT", Name = "Vermont"},
                    new State {Short = "WA", Name = "Washington"},
                    new State {Short = "WI", Name = "Wisconsin"},
                    new State {Short = "WV", Name = "West Virginia"},
                    new State {Short = "WY", Name = "Wyoming"},
                };
        }
    }
}