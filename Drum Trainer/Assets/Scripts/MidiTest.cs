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

public class MidiTest : MonoBehaviour
{
    private OutputDevice _outputDevice;
    private Playback _playback;

    private static IInputDevice _inputDevice;

    private void Start()
    {
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
        if (InputDevice.GetDevicesCount() > 0)
        {
            _inputDevice = InputDevice.GetByIndex(0);
            Debug.Log($"Initializing input device ...");
        }
        else
        {
            Debug.Log($"There is no input device presented in the system.");
            return;
        }

        _inputDevice.StartEventsListening();
        _inputDevice.EventReceived += OnEventReceived;

        Debug.Log("The device is listening: " + _inputDevice.IsListeningForEvents.ToString());
    }

    private void OnEventReceived(object sender, MidiEventReceivedEventArgs e)
    {
        var midiDevice = (MidiDevice)sender;
        Debug.Log($"Event received from '{midiDevice.Name}' at {DateTime.Now}: {e.Event}");
    }

    private void InitializeOutputDevice()
    {
        if (OutputDevice.GetDevicesCount() > 0)
        {
            _outputDevice = OutputDevice.GetByIndex(0);
            Debug.Log($"Initializing output device [{_outputDevice.Name}]...");
        }
        else
        {
            Debug.Log($"There is no output device presented in the system.");
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