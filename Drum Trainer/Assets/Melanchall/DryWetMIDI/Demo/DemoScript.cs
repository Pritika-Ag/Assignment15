using System;
using System.Linq;
using System.Text;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Composing;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;
using Melanchall.DryWetMidi.Standards;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
    private string OutputDeviceName;
    private string InputDeviceName;
    //private const string OutputDeviceName = "Microsoft GS Wavetable Synth";

    private OutputDevice _outputDevice;
    private Playback _playback;

    private static IInputDevice _inputDevice;

    private void Start()
    {
        Debug.Log("Hello");
        foreach (var outputDevice in OutputDevice.GetAll())
        {
            OutputDeviceName = outputDevice.Name;
            Console.WriteLine("Output: " + outputDevice.Name);
        }

        foreach (var inputDevice in InputDevice.GetAll())
        {
            InputDeviceName = inputDevice.Name;
            Console.WriteLine("Input: " + inputDevice.Name);
        }

        InitializeOutputDevice();
        InitializeInputDevice();
        //var midiFile = CreateTestFile();
        //InitializeFilePlayback(midiFile);
        //StartPlayback();
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Releasing playback and device...");

        if (_playback != null)
        {
            _playback.NotesPlaybackStarted -= OnNotesPlaybackStarted;
            _playback.NotesPlaybackFinished -= OnNotesPlaybackFinished;
            _playback.Dispose();
        }

        if (_outputDevice != null)
            _outputDevice.Dispose();

        if (_inputDevice != null)
            _inputDevice.Dispose();

        Debug.Log("Playback and device released.");
    }

    private void InitializeInputDevice()
    {
        if (InputDeviceName != null)
        {
            Debug.Log($"Initializing input device [{InputDeviceName}]...");

            var allInputDevices = InputDevice.GetAll();
            if (!allInputDevices.Any(d => d.Name == InputDeviceName))
            {
                var allDevicesList = string.Join(Environment.NewLine, allInputDevices.Select(d => $"  {d.Name}"));
                Debug.Log($"There is no [{InputDeviceName}] device presented in the system. Here the list of all device:{Environment.NewLine}{allDevicesList}");
                return;
            }

            _inputDevice = InputDevice.GetByName(InputDeviceName);
            Debug.Log($"Input device [{InputDeviceName}] initialized.");

            _inputDevice.StartEventsListening();

            _inputDevice.EventReceived += OnEventReceived;
            _inputDevice.StartEventsListening();

            Debug.Log("The device is listening: " + _inputDevice.IsListeningForEvents.ToString());
        }
    }

    private void OnEventReceived(object sender, MidiEventReceivedEventArgs e)
    {
        Debug.Log("Here");
        var midiDevice = (MidiDevice)sender;
        Debug.Log($"Event received from '{midiDevice.Name}' at {DateTime.Now}: {e.Event}");
    }

    private void InitializeOutputDevice()
    {
        if (OutputDeviceName != null) {
            

            if(OutputDevice.GetDevicesCount() > 0)
            {
                _outputDevice = OutputDevice.GetByIndex(0);
                
                Debug.Log($"Initializing output device [{_outputDevice.Name}]...");
            } else
            {
                Debug.Log($"There is no output device presented in the system.");
                return;
            }

            /*if (!allOutputDevices.Any(d => d.Name == OutputDeviceName))
            {
                var allDevicesList = string.Join(Environment.NewLine, allOutputDevices.Select(d => $"  {d.Name}"));
                Debug.Log($"There is no [{OutputDeviceName}] device presented in the system. Here the list of all device:{Environment.NewLine}{allDevicesList}");
                return;
            }

            _outputDevice = OutputDevice.GetByName(OutputDeviceName);*/
            Debug.Log($"Output device [{OutputDeviceName}] initialized.");
        }
    }

    private MidiFile CreateTestFile()
    {
        Debug.Log("Creating test MIDI file...");

        var patternBuilder = new PatternBuilder()
            .SetNoteLength(MusicalTimeSpan.Eighth)
            .SetVelocity(SevenBitNumber.MaxValue)
            .ProgramChange(GeneralMidiProgram.Harpsichord);

        foreach (var noteNumber in SevenBitNumber.Values)
        {
            patternBuilder.Note(Melanchall.DryWetMidi.MusicTheory.Note.Get(noteNumber));
        }

        var midiFile = patternBuilder.Build().ToFile(TempoMap.Default);
        Debug.Log("Test MIDI file created.");

        return midiFile;
    }

    private void InitializeFilePlayback(MidiFile midiFile)
    {
        Debug.Log("Initializing playback...");

        _playback = midiFile.GetPlayback(_outputDevice);
        _playback.Loop = true;
        _playback.NotesPlaybackStarted += OnNotesPlaybackStarted;
        _playback.NotesPlaybackFinished += OnNotesPlaybackFinished;
       
        Debug.Log("Playback initialized.");
    }

    private void StartPlayback()
    {
        Debug.Log("Starting playback...");
        _playback.Start();
    }

    private void OnNotesPlaybackFinished(object sender, NotesEventArgs e)
    {
        LogNotes("Notes finished:", e);
    }

    private void OnNotesPlaybackStarted(object sender, NotesEventArgs e)
    {
        LogNotes("Notes started:", e);
    }

    private void LogNotes(string title, NotesEventArgs e)
    {
        var message = new StringBuilder()
            .AppendLine(title)
            .AppendLine(string.Join(Environment.NewLine, e.Notes.Select(n => $"  {n}")))
            .ToString();
        Debug.Log(message.Trim());
    }
}