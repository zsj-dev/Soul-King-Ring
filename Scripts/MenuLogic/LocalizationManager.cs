using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public enum Language
{
    English,
    Chinese
}

public class LocalizationManager : MonoBehaviour
{
    static LocalizationManager m_instance;
    public static LocalizationManager Instance => m_instance;

    [SerializeField]
    Language m_currentLanguage;

    Dictionary<Language, TextAsset> m_localizationFileMap = new Dictionary<Language, TextAsset>();

    Dictionary<string, string> m_localizationData = new Dictionary<string, string>();

    void Awake()
    {
        CreateSingleton();

        LoadLocalisationFiles();
        LoadLocalizationData();
    }

    void CreateSingleton()
    {
        if(m_instance == null)
        {
            m_instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LoadLocalisationFiles()
    {
        foreach(Language language in Language.GetValues(typeof(Language)))
        {
            string textAssetPath = "Localization/" + language;
            TextAsset textAsset = (TextAsset)Resources.Load(textAssetPath);

            if(textAsset)
            {
                Debug.Log("Loaded TextAsset for Language: " + language);
                m_localizationFileMap[language] = textAsset;
            }
            else
            {
                Debug.LogWarning("Couldn't load TextAsset for Language: " + language);
            }
        }
    }

    void LoadLocalizationData()
    {
        TextAsset textAsset;

        if(m_localizationFileMap.ContainsKey(m_currentLanguage))
        {
            textAsset = m_localizationFileMap[m_currentLanguage];
        }
        else
        {
            Debug.LogError("Couldn't load language");
            return;
        }

        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(textAsset.text);

        XmlNodeList nodeList = xmlDocument.GetElementsByTagName("Entry");

        string key;
        string value;

        foreach(XmlNode node in nodeList)
        {
            key = node.FirstChild.InnerText;
            value = node.LastChild.InnerText;

            if(!m_localizationData.ContainsKey(key))
            {
                // Key doesn't exist
                m_localizationData.Add(key, value);

                Debug.Log("Added key: " + key + " with value: " + value);
            }
            else
            {
                Debug.LogWarning("Key already exists: " + key);
            }
        }
    }

    public string GetLocalizationValue(string key)
    {
        string value = "Value not found for key: " + key;
        if(m_localizationData.ContainsKey(key))
        {
            value = m_localizationData[key];
        }

        return value;
    }
}
