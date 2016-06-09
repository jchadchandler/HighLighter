# Social Media Link Viewer

##Purpose
This is a small WPF application to demonstrate the displaying of text in a highlighted form as it might be seen on social media 
such as Instagram. 

##Challenges
The requirement specified that the WPF RichTextBox be used as the control to display the highlighted text. This being a WPF application,
I immediately thought of have a UserText ViewModel where formatting logic would take place; however, the fact that the RichTextBox's
Document property is not a DependencyObject caused me to rethink my approach. While adding an attached FlowDocument property is 
one solution, the code began to get too complex to fulfill the brief requirements.

##Solution
I chose to sub-class the RichTextBox so that the new descendant was specifically designed to display social media type links. 
This simplified the code a great deal, kept formatting logic out of the MainWindow.xaml.cs file providing a DRY solution.

##In Conclusion
The project was great fun. It proved to be much more puzzling than first glance. 

