using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tridion.ContentManager;
using Tridion.ContentManager.CommunicationManagement;
using Tridion.ContentManager.ContentManagement;
using Tridion.ContentManager.Extensibility;
using Tridion.ContentManager.Extensibility.Events;
using Tridion.ContentManager.Publishing;
using Tridion.Logging;

namespace PublishComponentOnSave
{
    [TcmExtension("PublishComponentOnSaveExtension")]
    public class PublishComponentOnSave : TcmExtension
    {

        public PublishComponentOnSave()
        {
            Subscribe();
        }

        public void Subscribe()
        {
            EventSystem.SubscribeAsync<Component, CheckInEventArgs>(PublishComponent, EventPhases.TransactionCommitted);
            EventSystem.SubscribeAsync<Page, CheckInEventArgs>(PublishPage, EventPhases.TransactionCommitted);
            //EventSystem.Subscribe<Component, SaveEventArgs>(OnComponentSavePre, EventPhases.Initiated);

        }


        private void OnComponentSavePre(Component subject, SaveEventArgs args, EventPhases phase)
        {            
            if (subject.Schema.Title == "ExternalContent")
            {
               // subject.Title = "testEventSystem" + subject.Title;
            }
        }
        private void PublishComponent(Component subject, CheckInEventArgs args, EventPhases phase)
        {
            if (subject.Schema.Title == "productdetails" || subject.Schema.Title == "productintro")
            {

                //IdentifiableObject item = (IdentifiableObject)subject;
                //IEnumerable<IdentifiableObject> items = new List<IdentifiableObject>() { subject };                
                //RenderInstruction rndr = new RenderInstruction(subject.Session);
                //rndr.RenderMode = RenderMode.Publish;

                //ResolveInstruction resolveIns = new ResolveInstruction(subject.Session);
                //resolveIns.IncludeComponentLinks = true;

                //PublishInstruction pubInstruction = new PublishInstruction(subject.Session) 
                //{
                //    DeployAt = DateTime.Now,
                //    MaximumNumberOfRenderFailures = 5,
                //    RenderInstruction = rndr,
                //    ResolveInstruction = resolveIns,                   
                //    StartAt = DateTime.MinValue
                //};

                //TcmUri pubTargeturi = new TcmUri("tcm:0-4-65537");
                //PublicationTarget pubTarget = new PublicationTarget(pubTargeturi, subject.Session);
                //IEnumerable<PublicationTarget> lstPubTarget = new List<PublicationTarget>() { pubTarget };
                //PublishEngine.Publish(items, pubInstruction, lstPubTarget);

                List<IdentifiableObject> items = new List<IdentifiableObject>() { subject };
                TcmUri targetTypeUri = new TcmUri("tcm:0-4-65538");
                List<TargetType> targets = new List<TargetType> { new TargetType(targetTypeUri, subject.Session) };

                PublishInstruction publishInstruction = new PublishInstruction(subject.Session);
                PublishEngine.Publish(items, publishInstruction, targets,PublishPriority.Normal);

            }
        }

        private void PublishPage(Page subject, CheckInEventArgs args, EventPhases phase)
        {

            IdentifiableObject item = (IdentifiableObject)subject;
            IEnumerable<IdentifiableObject> items = new List<IdentifiableObject>() { subject };            
            PublishInstruction pubInstruction = new PublishInstruction(subject.Session);
            TcmUri pubTargeturi = new TcmUri("tcm:0-4-65537");
            PublicationTarget pubTarget = new PublicationTarget(pubTargeturi, subject.Session);
            IEnumerable<PublicationTarget> lstPubTarget = new List<PublicationTarget>() { pubTarget };

            PublishEngine.Publish(items, pubInstruction, lstPubTarget);
           

        }
    }
}
