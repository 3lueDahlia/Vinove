using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using ViNovE.Framework.Error;

namespace ViNovE.Framework.Data.UI
{
    [System.Serializable]
    public class InGameUISetting
    {
        ////////////////////////////////////////////////////////////////////////
        /// Class

        [System.Serializable]
        public class FontSetting
        {
            public Font _speakerNameFont;
            public int _speakerNameFontSize;
            public Color _speakerNameFontColor;

            public Font _dialogueFont;
            public int _dialogueFontSize;
            public Color _dialogueFontColor;

            public Font _dialogueButtonFont;
            public int _dialogueButtonFontSize;
            public Color _dialogueButtonFontColor;

            public Font _branchButtonFont;
            public int _branchButtonFontSize;
            public Color _branchButtonFontColor;
        }

        [System.Serializable]
        public class PanelBackgroundSetting
        {
            public Texture2D _speakerNamePanelImg;
            public Color _speakerNamePanelColor;

            public Texture2D _dialoguePanelImg;
            public Color _dialoguePanelColor;

            public Texture2D _dialogueButtonImg;
            public Color _dialogueButtonColor;

            public Texture2D _branchButtonImg;
            public Color _branchButtonColor;
        }

        ////////////////////////////////////////////////////////////////////////
        /// Variables

        FontSetting _fontSetting;
        PanelBackgroundSetting _panelBackgroundSetting;

        ////////////////////////////////////////////////////////////////////////
        /// Properties


        ////////////////////////////////
        /// Fonts

        public Font SpeakerNameFont
        {
            get { return _fontSetting._speakerNameFont; }
            set { _fontSetting._speakerNameFont = value; }
        }
        public int SpeakerNameFontSize
        {
            get { return _fontSetting._speakerNameFontSize; }
            set { _fontSetting._speakerNameFontSize = value; }
        }
        public Color SpeakerNameFontColor
        {
            get { return _fontSetting._speakerNameFontColor; }
            set { _fontSetting._speakerNameFontColor = value; }
        }

        public Font DialogueFont
        {
            get { return _fontSetting._dialogueFont; }
            set { _fontSetting._dialogueFont = value; }
        }
        public int DialogueFontSize
        {
            get { return _fontSetting._dialogueFontSize; }
            set { _fontSetting._dialogueFontSize = value; }
        }
        public Color DialogueFontColor
        {
            get { return _fontSetting._dialogueFontColor; }
            set { _fontSetting._dialogueFontColor = value; }
        }

        public Font DialogueButtonFont
        {
            get { return _fontSetting._dialogueButtonFont; }
            set { _fontSetting._dialogueButtonFont = value; }
        }
        public int DialogueButtonFontSize
        {
            get { return _fontSetting._dialogueButtonFontSize; }
            set { _fontSetting._dialogueButtonFontSize = value; }
        }
        public Color DialogueButtonFontColor
        {
            get { return _fontSetting._dialogueButtonFontColor; }
            set { _fontSetting._dialogueButtonFontColor = value; }
        }

        public Font BranchButtonFont
        {
            get { return _fontSetting._branchButtonFont; }
            set { _fontSetting._branchButtonFont = value; }
        }
        public int BranchButtonFontSize
        {
            get { return _fontSetting._branchButtonFontSize; }
            set { _fontSetting._branchButtonFontSize = value; }
        }
        public Color BranchButtonFontColor
        {
            get { return _fontSetting._branchButtonFontColor; }
            set { _fontSetting._branchButtonFontColor = value; }
        }
        ////////////////////////////////
        /// Panel Background

        public Texture2D SpeakerNamePanelImg
        {
            get { return _panelBackgroundSetting._speakerNamePanelImg; }
            set { _panelBackgroundSetting._speakerNamePanelImg = value; }
        }
        public Color SpeakerNamePanelColor
        {
            get { return _panelBackgroundSetting._speakerNamePanelColor; }
            set { _panelBackgroundSetting._speakerNamePanelColor = value; }
        }

        public Texture2D DialoguePanelImg
        {
            get { return _panelBackgroundSetting._dialoguePanelImg; }
            set { _panelBackgroundSetting._dialoguePanelImg = value; }
        }
        public Color DialoguePanelColor
        {
            get { return _panelBackgroundSetting._dialoguePanelColor; }
            set { _panelBackgroundSetting._dialoguePanelColor = value; }
        }

        public Texture2D DialogueButtonImg
        {
            get { return _panelBackgroundSetting._dialogueButtonImg; }
            set { _panelBackgroundSetting._dialogueButtonImg = value; }
        }
        public Color DialogueButtonColor
        {
            get { return _panelBackgroundSetting._dialogueButtonColor; }
            set { _panelBackgroundSetting._dialogueButtonColor = value; }
        }

        public Texture2D BranchButtonImg
        {
            get { return _panelBackgroundSetting._branchButtonImg; }
            set { _panelBackgroundSetting._branchButtonImg = value; }
        }
        public Color BranchButtonColor
        {
            get { return _panelBackgroundSetting._branchButtonColor; }
            set { _panelBackgroundSetting._branchButtonColor = value; }
        }

        ////////////////////////////////////////////////////////////////////////
        /// Func

        public InGameUISetting()
        {
            _fontSetting = new FontSetting();
            _panelBackgroundSetting = new PanelBackgroundSetting();

            _fontSetting._speakerNameFontSize = Constants.BASIC_FONT_SIZE;
            _fontSetting._speakerNameFontColor = new Color(0f, 0f, 0f, 1.0f);

            _fontSetting._dialogueFontSize = Constants.BASIC_FONT_SIZE;
            _fontSetting._dialogueFontColor = new Color(0f, 0f, 0f, 1.0f);

            _fontSetting._dialogueButtonFontSize = Constants.BASIC_FONT_SIZE;
            _fontSetting._dialogueButtonFontColor = new Color(0f, 0f, 0f, 1.0f);

            _fontSetting._branchButtonFontSize = Constants.BASIC_FONT_SIZE;
            _fontSetting._branchButtonFontColor = new Color(0f, 0f, 0f, 1.0f);

            _panelBackgroundSetting._speakerNamePanelColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            _panelBackgroundSetting._dialoguePanelColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            _panelBackgroundSetting._dialogueButtonColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            _panelBackgroundSetting._branchButtonColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
    }
}