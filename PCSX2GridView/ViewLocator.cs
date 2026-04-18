namespace PCSX2GridView
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Avalonia.Controls;
    using Avalonia.Controls.Templates;
    using PCSX2GridView.ViewModels;

    [ExcludeFromCodeCoverage(Justification = "Avalonia Scaffolding")]
    public class ViewLocator : IDataTemplate
    {
        public Control? Build(object? data)
        {
            if (data is null)
            {
                return null;
            }

            var name = data.GetType().FullName!.Replace("ViewModel", "View");
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type) !;
            }
            else
            {
                return new TextBlock { Text = "Not Found: " + name };
            }
        }

        public bool Match(object? data)
        {
            return data is ViewModelBase;
        }
    }
}