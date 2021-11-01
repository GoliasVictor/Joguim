#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace DebugVisualizer
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public abstract class ExtractedData
    {
        [JsonProperty("kind")]
        public Dictionary<string, bool> Kind => Tags.ToDictionary( T => T, T => true);

        [JsonIgnore]
        public abstract string[] Tags { get; }

        public override string ToString() => JsonConvert.SerializeObject(this);
        
    }

    static public class VisualizeHelper {
        public static string Visualize(this IEnumerable<double> line)
        {
            return JsonConvert.SerializeObject(new Plotly(new IEnumerable<double>[]{line}));
        }
    }

    public class Plotly : ExtractedData
    {

        public override string[] Tags => new string[] { "plotly" };
        [JsonProperty("data")]
        public Line[] data;

		public Plotly(IEnumerable<IEnumerable<double>> data)
		{
			this.data = data.Select(line => new Line(line)).ToArray();
		}
        [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
		public class Line 
        {
            [JsonProperty("y")]
            public double[] y;
			public Line(IEnumerable<double> y)
			{
				this.y = y.ToArray();
			}
		}
        
    }
}

