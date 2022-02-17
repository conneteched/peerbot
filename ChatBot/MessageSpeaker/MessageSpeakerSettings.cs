﻿using Google.Cloud.TextToSpeech.V1;
using System;
using System.Collections.Generic;
using System.Text;


namespace ChatBot.MessageSpeaker
{
    public class MessageSpeakerSettings
    {
        public string twitchUsername = "";
        public double speakingRate = 0.8;
        public double pitch = 0;
        public string languageCode = "en-GB";
        public SsmlVoiceGender gender = SsmlVoiceGender.Female;
        public bool isSpeechEnabled = false;

        public enum eDialects
        {
            australian,
            irish,
            southAfrican,
            british,
            american,
            french,
            japanese,
            canadianFrench,
            none
        }

        public void SetIsSpeechEnabled(string argumet)
        {
            if (argumet.StartsWith("enable"))
                isSpeechEnabled = true;
            if (argumet.StartsWith("disable"))
                isSpeechEnabled = false;
        }

        public bool SetGender(string argument)
        {            switch (argument)
            {
                case "man":
                    gender = SsmlVoiceGender.Male;
                    return true;
                    break;
                case "woman":
                    gender = SsmlVoiceGender.Female;
                    return true;
                    break;
                case "unspecified":
                    gender = SsmlVoiceGender.Unspecified;
                    return true;
                    break;
                case "neutral":
                    gender = SsmlVoiceGender.Neutral;
                    return true;
                    break;
                default:
                    return false;
            }
        }

        public void SetSpeed(double value)
        {
            speakingRate = value;
        }

        public void SetPitch(double value)
        {
            pitch = value;
        }

        public bool SetLanguage(string dialect)
        {
            switch (MessageSpeakerSettings.GetDialectFromString(dialect))
            {
                case eDialects.australian:
                    languageCode = "en-AU";
                    return true;
                    break;
                case eDialects.irish:
                    languageCode = "en-IE";
                    return true;
                    break;
                case eDialects.southAfrican:
                    languageCode = "en-ZA";
                    return true;
                    break;
                case eDialects.british:
                    languageCode = "en-GB";
                    return true;
                    break;
                case eDialects.american:
                    languageCode = "en-US";
                    return true;
                    break;
                case eDialects.french:
                    languageCode = "fr-FR";
                    return true;
                case eDialects.japanese:
                    languageCode = "ja-JP";
                    return true;
                case eDialects.canadianFrench:
                    languageCode = "fr-CA";
                    return true;
                default:
                    return false;
            }
        }

        private VoiceSelectionParams GetVoiceParams()
        {
            VoiceSelectionParams voiceParams = new VoiceSelectionParams();
            voiceParams.LanguageCode = languageCode;
            voiceParams.SsmlGender = gender;

            // Hand picked voice sounds by region - if the enum isnt covered in this list just use the default voice name
            if (gender == SsmlVoiceGender.Male)
            {
                switch (GetDialectFromString(languageCode))
                {
                    case eDialects.australian:
                        voiceParams.Name = "en-AU-Wavenet-D";
                        break;
                    case eDialects.british:
                        voiceParams.Name = "en-GB-Wavenet-D";
                        break;
                    case eDialects.american:
                        voiceParams.Name = "en-US-Wavenet-B";
                        break;
                    case eDialects.french:
                        voiceParams.Name = "fr-FR-Wavenet-B";
                        break;
                    case eDialects.japanese:
                        voiceParams.Name = "ja-JP-Wavenet-D";
                        break;
                    case eDialects.canadianFrench:
                        voiceParams.Name = "fr-CA-Standard-D";
                        break;
                }
            }
            else if (gender == SsmlVoiceGender.Female)
            {
                switch (GetDialectFromString(languageCode))
                {
                    case eDialects.australian:
                        voiceParams.Name = "en-AU-Wavenet-C";
                        break;
                    case eDialects.british:
                        voiceParams.Name = "en-GB-Wavenet-F";
                        break;
                    case eDialects.american:
                        voiceParams.Name = "en-US-Standard-F";
                        break;
                    case eDialects.french:
                        voiceParams.Name = "fr-FR-Wavenet-A";
                        break;
                    case eDialects.japanese:
                        voiceParams.Name = "ja-JP-Wavenet-C";
                        break;
                }
            }

            return voiceParams;
        }

        public static eDialects GetDialectFromString(string dialect)
        {
            switch (dialect)
            {
                case "australian":
                    return eDialects.australian;
                case "irish":
                    return eDialects.irish;
                case "south african":
                    return eDialects.southAfrican;
                case "british":
                    return eDialects.british;
                case "american":
                    return eDialects.american;
                case "french":
                case "fr-FR":
                    return eDialects.french;
                case "japanese":
                    return eDialects.japanese;
                case "canadian french":
                case "fr-CA":
                    return eDialects.canadianFrench;
                default:
                    return eDialects.none;
            }
        }


        private AudioConfig GetAudioConfig()
        {
            AudioConfig audioConfig = new AudioConfig();
            audioConfig.AudioEncoding = AudioEncoding.Mp3;
            audioConfig.SpeakingRate = speakingRate;
            audioConfig.Pitch = pitch;
            return audioConfig;
        }

        public SynthesizeSpeechRequest GetSpeechRequest(string Username, string Message)
        {
            var settings = MessageSpeakerSettingsManager.GetSettingsFromStorage(Username);
            var config = settings.GetAudioConfig();
            var voiceParams = settings.GetVoiceParams();

            SynthesizeSpeechRequest request = new SynthesizeSpeechRequest();
            request.AudioConfig = config;
            request.Voice = voiceParams;

            // Build the input and add it
            SynthesisInput input = new SynthesisInput();
            input.Text = Message;
            request.Input = input;

            return request;
        }
    }
}