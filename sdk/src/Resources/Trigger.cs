using System.Collections.Generic;
using Newtonsoft.Json;
using Omnigage.Core;

namespace Omnigage.Resource
{
    public class TriggerKind
    {
        public const string Play = "play";
        public const string Say = "say";
        public const string Gather = "gather";
        public const string Prompt = "prompt";
        public const string Conference = "conference";
        public const string Enqueue = "enqueue";
        public const string Notify = "notify";
        public const string Dial = "dial";
        public const string Pause = "pause";
        public const string Record = "record";
        public const string Activity = "activity";
        public const string Hangup = "hangup";
    }

    public class TriggerOnEvent
    {
        public const string VoiceQueued = "voice-queued";
        public const string VoiceRinging = "voice-ringing";
        public const string VoiceAnswered = "voice-answered";
        public const string VoiceHuman = "voice-human";
        public const string VoiceMachine = "voice-machine";
        public const string VoiceHangup = "voice-hangup";
        public const string VoiceUnanswered = "voice-unanswered";
        public const string VoiceBusy = "voice-busy";
        public const string VoiceFailed = "voice-failed";
        public const string VoiceCanceled = "voice-canceled";
        public const string VoiceCompleted = "voice-completed";
        public const string EmailQueued = "email-queued";
        public const string EmailSent = "email-sent";
        public const string EmailDelivered = "email-delivered";
        public const string EmailOpen = "email-open";
        public const string EmailClick = "email-click";
        public const string EmailUnsubscribe = "email-unsubscribe";
        public const string EmailBounce = "email-bounce";
        public const string EmailComplaint = "email-complaint";
        public const string EmailFailed = "email-failed";
        public const string TextQueued = "text-queued";
        public const string TextSent = "text-sent";
        public const string TextDelivered = "text-delivered";
        public const string TextUndelivered = "text-undelivered";
        public const string TextFailed = "text-failed";
    }

    /// <summary>
    /// Resource: `/triggers` - https://omnigage.docs.apiary.io/#reference/engagement-resources/trigger-collection
    /// </summary>
    public class TriggerResource : Adapter
    {
        public override string Type { get; } = "triggers";

        public string Kind;

        [JsonProperty(propertyName: "on-event")]
        public string OnEvent;

        public int Timeout;

        public bool Trim;

        public bool Transcribe;

        public bool Beep;

        [JsonProperty(propertyName: "digit-stop")]
        public string DigitStop;

        // Use with `TriggerKind.Play` and `TriggerKind.Say`
        public int Loop;

        // Use with `TriggerKind.Pause` and `TriggerKind.Record`
        public int Length;

        // Use with `TriggerKind.Dial`
        [JsonProperty(propertyName: "phone-number")]
        public int PhoneNumber;

        // Use with `TriggerKind.Activity`
        [JsonProperty(propertyName: "delay-in-seconds")]
        public int DelayInSeconds;

        // Use with `TriggerKind.Prompt`
        public string Digits;

        public string Speech;

        // Use with `TriggerKind.Gather`
        public List<string> Input;

        [JsonProperty(propertyName: "dtmf-digit-count")]
        public int DtmfDigitCount;

        [JsonProperty(propertyName: "dtmf-digit-stop")]
        public string DtmfDigitStop;

        [JsonProperty(propertyName: "voice-template")]
        public VoiceTemplateResource VoiceTemplate;

        public ActivityResource Activity;

        public TriggerResource Parent;

        public List<TriggerResource> Children { get; set; }

        public ConferenceResource Conference;
    }
}
