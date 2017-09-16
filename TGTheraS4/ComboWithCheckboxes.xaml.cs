using System.Collections;
using System.Windows;

namespace TheraS5
{
    /// <summary>
    ///     Interaction logic for ComboWithCheckboxes.xaml
    /// </summary>
    public partial class ComboWithCheckboxes
    {
        public ComboWithCheckboxes()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Whenever a CheckBox is checked, change the text displayed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            SetText();
        }

        /// <summary>
        ///     Set the text property of this control (bound to the ContentPresenter of the ComboBox)
        /// </summary>
        private void SetText()
        {
            Text = ItemsSource != null ? ItemsSource.ToString() : DefaultText;

            // set DefaultText if nothing else selected
            if (string.IsNullOrEmpty(Text))
            {
                Text = DefaultText;
            }
        }

        #region Dependency Properties

        /// <summary>
        ///     Gets or sets a collection used to generate the content of the ComboBox
        /// </summary>
        public object ItemsSource
        {
            get => GetValue(ItemsSourceProperty);

            set
            {
                CheckableCombo.ItemsSource = (IEnumerable) value;

                SetValue(ItemsSourceProperty, value);

                SetText();
            }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(object), typeof(ComboWithCheckboxes),
                new UIPropertyMetadata(null));

        /// <summary>
        ///     Gets or sets the text displayed in the ComboBox
        /// </summary>
        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ComboWithCheckboxes),
                new UIPropertyMetadata(string.Empty));


        /// <summary>
        ///     Gets or sets the text displayed in the ComboBox if there are no selected items
        /// </summary>
        public string DefaultText
        {
            get => (string) GetValue(DefaultTextProperty);
            set => SetValue(DefaultTextProperty, value);
        }

        // Using a DependencyProperty as the backing store for DefaultText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultTextProperty =
            DependencyProperty.Register("DefaultText", typeof(string), typeof(ComboWithCheckboxes),
                new UIPropertyMetadata(string.Empty));

        #endregion
    }
}