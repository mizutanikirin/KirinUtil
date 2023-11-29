using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;

namespace KirinUtil.Demo
{
    public class SetTMPDemo : MonoBehaviour
    {
        [SerializeField] private SetTMP setTMP;
        [SerializeField] private TextMeshProUGUI xmlText;

        // Start is called before the first frame update
        void Start()
        {
            ReadXmlData();
        }

        // xml����ݒ��ǂݎ��xmlText�ɃZ�b�g����܂ł̗���
        private void ReadXmlData()
        {
            string xmlData =
                "<text x=\"0\" y=\"0\" align=\"Center\" width=\"300\" sizeMin=\"20\" sizeMax=\"50\" color=\"#ff0000\" " +
                "outline=\"True\" outlineColor=\"#ffffff\" outlineThickness=\"0.5\">ABCD 0123</text>";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlData);
            XmlNodeList nodes = xmlDoc.GetElementsByTagName("text");

            // xml��TMPData�^�ɕϊ�
            TMPData textData = setTMP.Xml2TMPData(nodes);

            // xmlText��textData�ݒ��K�p
            setTMP.Custum(xmlText, textData);
        }
    }
}
