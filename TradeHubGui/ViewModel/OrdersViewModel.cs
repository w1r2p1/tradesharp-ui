﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms.VisualStyles;
using TradeHub.Common.Core.Constants;
using TradeHub.Common.Core.DomainModels;
using TradeHubGui.Common.Constants;
using TradeHubGui.Common.Models;
using TradeHubGui.Dashboard.Services;
using OrderExecutionProvider = TradeHubGui.Common.Models.OrderExecutionProvider;
using TradeHubConstants = TradeHub.Common.Core.Constants;

namespace TradeHubGui.ViewModel
{
    public class OrdersViewModel : BaseViewModel
    {
        #region Fields

        private string _filterString;

        private OrderExecutionProvider _selectedProvider;

        private ICollectionView _orderCollection;

        private ObservableCollection<OrderDetails> _orders;
        private ObservableCollection<PositionStatistics> _positionStatisticsCollection; 
        private ObservableCollection<OrderExecutionProvider> _executionProviders; 
        
        #endregion

        #region Constructor

        public OrdersViewModel()
        {
            _orders = new ObservableCollection<OrderDetails>();
            _executionProviders = new ObservableCollection<OrderExecutionProvider>();
            _positionStatisticsCollection = new ObservableCollection<PositionStatistics>();

            _orderCollection = CollectionViewSource.GetDefaultView(_orders);
            if(_orderCollection != null && _orderCollection.CanSort == true)
            {
                _orderCollection.SortDescriptions.Clear();
                _orderCollection.SortDescriptions.Add(new SortDescription("Security.Symbol", ListSortDirection.Ascending));
                _orderCollection.Filter = OrderCollectionFilter;
            }

            PopulateExecutionProviders();

            ////Dummy Function
            //TestCodeToAddOrderDetailsToExecutionProvider();
        }
        
        #endregion

        #region Properties

        /// <summary>
        /// Collection of OrderDetails
        /// </summary>
        public ObservableCollection<OrderDetails> Orders
        {
            get { return _orders; }
            set
            {
                if (_orders != value)
                {
                    _orders = value;
                    OnPropertyChanged("Orders");
                }
            }
        }

        /// <summary>
        /// ICollectionView used as items source for DataGrid
        /// </summary>
        public ICollectionView OrderCollection
        {
            get { return _orderCollection; }
        }

        /// <summary>
        /// Filter string used to find OrderDetails by symbol
        /// </summary>
        public string FilterString
        {
            get { return _filterString; }
            set
            {
                _filterString = value;
                OnPropertyChanged("FilterString");
                _orderCollection.Refresh();
            }
        }

        /// <summary>
        /// Contains all available providers
        /// </summary>
        public ObservableCollection<OrderExecutionProvider> ExecutionProviders
        {
            get { return _executionProviders; }
            set
            {
                _executionProviders = value;
                OnPropertyChanged("ExecutionProviders");
            }
        }

        /// <summary>
        /// Contains Position Stats for all traded symbols for the selected Execution Provider
        /// </summary>
        public ObservableCollection<PositionStatistics> PositionStatisticsCollection
        {
            get { return _positionStatisticsCollection; }
            set
            {
                _positionStatisticsCollection = value;
                OnPropertyChanged("PositionStatisticsCollection");
            }
        }

        /// <summary>
        /// Currently User selected Order Execution Provider
        /// </summary>
        public OrderExecutionProvider SelectedProvider
        {
            get { return _selectedProvider; }
            set
            {
                _selectedProvider = value;
                if (value != null)
                    PopulateOrderDetails();

                OnPropertyChanged("SelectedProvider");
            }
        }

        #endregion

        #region Commands
        #endregion

        #region Methods

        private bool OrderCollectionFilter(object item)
        {
            OrderDetails orderDetails = item as OrderDetails;
            if (_filterString != null)
            {
                return orderDetails.Security.Symbol.ToLower().Contains(_filterString);
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Populates values in Execution Providers Collection
        /// </summary>
        private void PopulateExecutionProviders()
        {
            foreach (var provider in ProvidersController.OrderExecutionProviders)
            {
                _executionProviders.Add(provider);
            }
        }

        /// <summary>
        /// Displays Order Details for the selected Execution Provider
        /// </summary>
        private void PopulateOrderDetails()
        {
            // Clear current values
            Orders.Clear();
            PositionStatisticsCollection.Clear();
            Orders = new ObservableCollection<OrderDetails>();
            PositionStatisticsCollection = new ObservableCollection<PositionStatistics>();

            // Set New Values
            Orders = SelectedProvider.OrdersCollection;
            PositionStatisticsCollection = SelectedProvider.PositionStatisticsCollection;

            //// Populate Order Details
            //foreach (var orderDetails in SelectedProvider.OrdersCollection)
            //{
            //    Orders.Add(orderDetails);
            //}

            //// Populate Position Details
            //foreach (var positionStats in SelectedProvider.PositionStatisticsCollection.Values)
            //{
            //    PositionStatisticsCollection.Add(positionStats);
            //}

            // Select the 1st instance from DataGrid
            //SelectedInstance = Instances.Count > 0 ? Instances[0] : null;
        }

        #endregion

        #region Events
        #endregion

        private void TestCodeToAddOrderDetailsToExecutionProvider()
        {
            OrderDetails orderDetails = new OrderDetails();
            orderDetails.ID = "01";
            orderDetails.Type = OrderType.Market;
            orderDetails.Quantity = 10;
            orderDetails.Security = new Security() { Symbol = "AAPL" };
            orderDetails.Side = OrderSide.BUY;
            orderDetails.Provider = TradeHubConstants.OrderExecutionProvider.Simulated;
            orderDetails.Status = OrderStatus.EXECUTED;

            FillDetail fillDetail = new FillDetail();
            fillDetail.FillPrice = 123.76M;
            fillDetail.FillQuantity = 10;
            fillDetail.FillType = ExecutionType.Fill;

            orderDetails.FillDetails.Add(fillDetail);

            foreach (var provider in ExecutionProviders)
            {
                if (provider.ProviderName.Equals(TradeHubConstants.OrderExecutionProvider.Simulated))
                {
                    provider.AddOrder(orderDetails);
                    provider.UpdatePosition(orderDetails);
                }
            }
        }
    }
}
