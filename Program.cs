using System;
using System.Linq;
using System.Threading.Tasks;

namespace dpOra2Pg
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            if (!args.Any())
            {
                Console.WriteLine("No config file supplied. Exiting.");
                return;
            }

            SettingsController sc = new SettingsController(args[0]);

            Transition transition = new Transition(sc.Settings);
            await transition.ExecuteTransition();

            Console.ReadLine();
        }

    }
}
