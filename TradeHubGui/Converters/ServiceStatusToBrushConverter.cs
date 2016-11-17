/***************************************************************************** 
* Copyright 2016 Aurora Solutions 
* 
*    http://www.aurorasolutions.io 
* 
* Aurora Solutions is an innovative services and product company at 
* the forefront of the software industry, with processes and practices 
* involving Domain Driven Design(DDD), Agile methodologies to build 
* scalable, secure, reliable and high performance products.
* 
* TradeSharp is a C# based data feed and broker neutral Algorithmic 
* Trading Platform that lets trading firms or individuals automate 
* any rules based trading strategies in stocks, forex and ETFs. 
* TradeSharp allows users to connect to providers like Tradier Brokerage, 
* IQFeed, FXCM, Blackwood, Forexware, Integral, HotSpot, Currenex, 
* Interactive Brokers and more. 
* Key features: Place and Manage Orders, Risk Management, 
* Generate Customized Reports etc 
* 
* Licensed under the Apache License, Version 2.0 (the "License"); 
* you may not use this file except in compliance with the License. 
* You may obtain a copy of the License at 
* 
*    http://www.apache.org/licenses/LICENSE-2.0 
* 
* Unless required by applicable law or agreed to in writing, software 
* distributed under the License is distributed on an "AS IS" BASIS, 
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
* See the License for the specific language governing permissions and 
* limitations under the License. 
*****************************************************************************/


﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using TradeHubGui.Common.Constants;

namespace TradeHubGui.Converters
{
    public class ServiceStatusToBrushConverter : IValueConverter
    {
        /// <summary>
        /// Converts order status to SolidColorBrush
        /// </summary>
        /// <param name="value">ServiceStatus</param>
        /// <param name="targetType">not used</param>
        /// <param name="parameter">not used</param>
        /// <param name="culture">not used</param>
        /// <returns>System.Windows.Media.SolidColorBrush</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value != DependencyProperty.UnsetValue)
            {
                var status = (ServiceStatus)value;

                // Return certain brush depending on OrderStatus
                if (status.Equals(ServiceStatus.Disabled))
                {
                    return Application.Current.Resources["GrayBrush"];
                }
                else if (status.Equals(ServiceStatus.Starting))
                {
                    return Application.Current.Resources["MediumTurquoiseBrush"];
                }
                else if (status.Equals(ServiceStatus.Running))
                {
                    return Application.Current.Resources["GreenBrush"];
                }
                else if (status.Equals(ServiceStatus.Stopping))
                {
                    return Application.Current.Resources["OrangeBrush"];
                }
                else if (status.Equals(ServiceStatus.Stopped))
                {
                    return Application.Current.Resources["RedBrush"];
                }
            }

            // default color is Gray
            return Application.Current.Resources["GrayBrush"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
