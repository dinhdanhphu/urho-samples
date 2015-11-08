﻿using System;
using System.Linq;

namespace Urho.Samples.Desktop
{
	class Program
	{
		static Type[] samples;

		static void Main(string[] args)
		{
			// Actually you can launch any sample using this snippet:
			//
			//   UrhoEngine.Init(pathToAssets);
			//   new Water(new Context()).Run();
			//   return;

			FindAvailableSamplesAndPrint();
			Type selectedSampleType = null;

			if (args.Length > 0)
			{
				// try to get a desired sample's number to run via command line args:
				selectedSampleType = ParseSampleFromNumber(args[0]);
			}
			if (Environment.OSVersion.Platform == PlatformID.Win32NT)
			{
				while (selectedSampleType == null)
				{
					WriteLine("Enter a sample number:", ConsoleColor.White);
					selectedSampleType = ParseSampleFromNumber(Console.ReadLine());
				}
			}
			else if (selectedSampleType == null)
			{
				selectedSampleType = typeof(ToonTown); //show ToonTown sample by default for OS X if args are empty.
			}

			var resourcesDirectory = @"../../Assets";
			//special assets for AtomicEngine based samples:
			if (selectedSampleType == typeof (ToonTown))
			{
				resourcesDirectory = @"../../Assets/AtomicEngineAssets";
			}

			UrhoEngine.Init(resourcesDirectory);
			var game = (Application) Activator.CreateInstance(selectedSampleType, new Context());
			var exitCode = game.Run();
			WriteLine($"Exit code: {exitCode}. Press any key to exit...", ConsoleColor.DarkYellow);
			Console.ReadKey();
		}

		static Type ParseSampleFromNumber(string input)
		{
			int number;
			if (!int.TryParse(input, out number))
			{
				WriteLine("Invalid format.", ConsoleColor.Red);
				return null;
			}

			if (number >= samples.Length || number < 0)
			{
				WriteLine("Invalid number.", ConsoleColor.Red);
				return null;
			}

			return samples[number];
		}

		static void FindAvailableSamplesAndPrint()
		{
			samples = typeof(Sample).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Application)) && t != typeof(Sample)).ToArray();
			int blockSize = samples.Length / 2;
			for (int i = 0; i < blockSize; i++)
				WriteLine(string.Format("{2,2}. {0,-30}{3,2}. {1,-30}", samples[i].Name, samples[i + blockSize].Name, i, i + blockSize), ConsoleColor.DarkGray);
			if (blockSize * 2 < samples.Length)
				WriteLine($"{samples.Length - 1,36}. {samples[samples.Length - 1].Name}", ConsoleColor.DarkGray);
			Console.WriteLine();
		}

		static void WriteLine(string text, ConsoleColor consoleColor)
		{
			var color = Console.ForegroundColor;
			Console.ForegroundColor = consoleColor;
			Console.WriteLine(text);
			Console.ForegroundColor = color;
		}
	}
}
