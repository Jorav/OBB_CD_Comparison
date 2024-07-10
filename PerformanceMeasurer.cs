using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace OBB_CD_Comparison
{
    public class PerformanceMeasurer
    {
        public List<double> elapsedTimes;

        public PerformanceMeasurer()
        {
            elapsedTimes = new List<double>();
        }

        public void Update (GameTime gameTime)
        {
            elapsedTimes.Add(gameTime.ElapsedGameTime.TotalSeconds);
        }

        public void Exit()
        {
            // Set a variable to the Documents path.
            string docPath =
              Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Write the string array to a new file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "elapsedTimes.txt")))
            {
                foreach (double time in elapsedTimes)
                    outputFile.WriteLine(time.ToString());
            }
        }
    }
}
