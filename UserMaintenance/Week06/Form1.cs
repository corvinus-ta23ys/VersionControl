﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using Week06.Entities;
using Week06.MnbServiceReference;

namespace Week06
{
    public partial class Form1 : Form
    {
        BindingList<RateData> Rates = new BindingList<RateData>();
        BindingList<string> Currencies = new BindingList<string>();

        public Form1()
        {
            InitializeComponent();
            comboBox1.DataSource = Currencies;
            GetCurrencies();
            RefreshData();
        }

        public void GetCurrencies()
        {
            var mnbService = new MNBArfolyamServiceSoapClient();

            var request = new GetExchangeRatesRequestBody()
            {
                currencyNames = comboBox1.SelectedItem.ToString(),
                startDate = dateTimePicker1.Value.ToString(),
                endDate = dateTimePicker2.Value.ToString()
            };

            var response = mnbService.GetExchangeRates(request);

            var result = response.GetExchangeRatesResult;
            //Innentől sajnos nem tudtam megoldani a feladatot.
        }

            public void GetExchangeRates()
        {
            var mnbService = new MNBArfolyamServiceSoapClient();

            var request = new GetExchangeRatesRequestBody()
            {
                currencyNames = comboBox1.SelectedItem.ToString(),
                startDate = dateTimePicker1.Value.ToString(),
                endDate = dateTimePicker2.Value.ToString()
            };

            var response = mnbService.GetExchangeRates(request);

            var result = response.GetExchangeRatesResult;

            var xml = new XmlDocument();
            xml.LoadXml(result);

            foreach (XmlElement element in xml.DocumentElement)
            {
                var rate = new RateData();
                Rates.Add(rate);

                // Dátum
                rate.Date = DateTime.Parse(element.GetAttribute("date"));

                // Valuta
                var childElement = (XmlElement)element.ChildNodes[0];
                rate.Currency = childElement.GetAttribute("curr");

                // Érték
                var unit = decimal.Parse(childElement.GetAttribute("unit"));
                var value = decimal.Parse(childElement.InnerText);
                if (unit != 0)
                    rate.Value = value / unit;
            }
        }
        public void ChartFeladat()
        {
            chartRateData.DataSource = Rates;

            var series = chartRateData.Series[0];
            series.ChartType = SeriesChartType.Line;
            series.XValueMember = "Date";
            series.YValueMembers = "Value";
            series.BorderWidth = 2;

            var legend = chartRateData.Legends[0];
            legend.Enabled = false;

            var chartArea = chartRateData.ChartAreas[0];
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.IsStartedFromZero = false;
        }

        public void RefreshData()
        {
            Rates.Clear();
            GetExchangeRates();
            ChartFeladat();

            dataGridView1.DataSource = Rates;
            chartRateData.DataSource = Rates;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshData();
        }
        /*public void processXML()
{
var xml = new XmlDocument();
xml.LoadXml(result);

foreach (XmlElement element in xml.DocumentElement)
{
var rate = new RateData();
Rates.Add(rate);

// Dátum
rate.Date = DateTime.Parse(element.GetAttribute("date"));

// Valuta
var childElement = (XmlElement)element.ChildNodes[0];
rate.Currency = childElement.GetAttribute("curr");

// Érték
var unit = decimal.Parse(childElement.GetAttribute("unit"));
var value = decimal.Parse(childElement.InnerText);
if (unit != 0)
rate.Value = value / unit;
}*/
    }
        

    }

