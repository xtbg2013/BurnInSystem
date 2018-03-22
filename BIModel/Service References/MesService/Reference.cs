﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BIModel.MesService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="MesService.IMesService")]
    public interface IMesService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/GetWorkStep", ReplyAction="http://tempuri.org/IMesService/GetWorkStepResponse")]
        BIModel.MesService.GetWorkStepResponse GetWorkStep(BIModel.MesService.GetWorkStepRequest request);
        
        // CODEGEN: Generating message contract since the operation has multiple return values.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/GetWorkStep", ReplyAction="http://tempuri.org/IMesService/GetWorkStepResponse")]
        System.Threading.Tasks.Task<BIModel.MesService.GetWorkStepResponse> GetWorkStepAsync(BIModel.MesService.GetWorkStepRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/MoveStandard", ReplyAction="http://tempuri.org/IMesService/MoveStandardResponse")]
        BIModel.MesService.MoveStandardResponse MoveStandard(BIModel.MesService.MoveStandardRequest request);
        
        // CODEGEN: Generating message contract since the operation has multiple return values.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/MoveStandard", ReplyAction="http://tempuri.org/IMesService/MoveStandardResponse")]
        System.Threading.Tasks.Task<BIModel.MesService.MoveStandardResponse> MoveStandardAsync(BIModel.MesService.MoveStandardRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/Hold", ReplyAction="http://tempuri.org/IMesService/HoldResponse")]
        BIModel.MesService.HoldResponse Hold(BIModel.MesService.HoldRequest request);
        
        // CODEGEN: Generating message contract since the operation has multiple return values.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/Hold", ReplyAction="http://tempuri.org/IMesService/HoldResponse")]
        System.Threading.Tasks.Task<BIModel.MesService.HoldResponse> HoldAsync(BIModel.MesService.HoldRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/GetStepState", ReplyAction="http://tempuri.org/IMesService/GetStepStateResponse")]
        BIModel.MesService.GetStepStateResponse GetStepState(BIModel.MesService.GetStepStateRequest request);
        
        // CODEGEN: Generating message contract since the operation has multiple return values.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/GetStepState", ReplyAction="http://tempuri.org/IMesService/GetStepStateResponse")]
        System.Threading.Tasks.Task<BIModel.MesService.GetStepStateResponse> GetStepStateAsync(BIModel.MesService.GetStepStateRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/GetCocInfoBySn", ReplyAction="http://tempuri.org/IMesService/GetCocInfoBySnResponse")]
        BIModel.MesService.GetCocInfoBySnResponse GetCocInfoBySn(BIModel.MesService.GetCocInfoBySnRequest request);
        
        // CODEGEN: Generating message contract since the operation has multiple return values.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/GetCocInfoBySn", ReplyAction="http://tempuri.org/IMesService/GetCocInfoBySnResponse")]
        System.Threading.Tasks.Task<BIModel.MesService.GetCocInfoBySnResponse> GetCocInfoBySnAsync(BIModel.MesService.GetCocInfoBySnRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetWorkStep", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class GetWorkStepRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public string sn;
        
        public GetWorkStepRequest() {
        }
        
        public GetWorkStepRequest(string sn) {
            this.sn = sn;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetWorkStepResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class GetWorkStepResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public bool GetWorkStepResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public string workStep;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=2)]
        public string errorMessage;
        
        public GetWorkStepResponse() {
        }
        
        public GetWorkStepResponse(bool GetWorkStepResult, string workStep, string errorMessage) {
            this.GetWorkStepResult = GetWorkStepResult;
            this.workStep = workStep;
            this.errorMessage = errorMessage;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="MoveStandard", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class MoveStandardRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public string sn;
        
        public MoveStandardRequest() {
        }
        
        public MoveStandardRequest(string sn) {
            this.sn = sn;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="MoveStandardResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class MoveStandardResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public bool MoveStandardResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public string errorMessage;
        
        public MoveStandardResponse() {
        }
        
        public MoveStandardResponse(bool MoveStandardResult, string errorMessage) {
            this.MoveStandardResult = MoveStandardResult;
            this.errorMessage = errorMessage;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Hold", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class HoldRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public string sn;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public string reason;
        
        public HoldRequest() {
        }
        
        public HoldRequest(string sn, string reason) {
            this.sn = sn;
            this.reason = reason;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="HoldResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class HoldResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public bool HoldResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public string errorMessage;
        
        public HoldResponse() {
        }
        
        public HoldResponse(bool HoldResult, string errorMessage) {
            this.HoldResult = HoldResult;
            this.errorMessage = errorMessage;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetStepState", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class GetStepStateRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public string sn;
        
        public GetStepStateRequest() {
        }
        
        public GetStepStateRequest(string sn) {
            this.sn = sn;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetStepStateResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class GetStepStateResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public bool GetStepStateResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public string StepState;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=2)]
        public string errorMessage;
        
        public GetStepStateResponse() {
        }
        
        public GetStepStateResponse(bool GetStepStateResult, string StepState, string errorMessage) {
            this.GetStepStateResult = GetStepStateResult;
            this.StepState = StepState;
            this.errorMessage = errorMessage;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetCocInfoBySn", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class GetCocInfoBySnRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public string sn;
        
        public GetCocInfoBySnRequest() {
        }
        
        public GetCocInfoBySnRequest(string sn) {
            this.sn = sn;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetCocInfoBySnResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class GetCocInfoBySnResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public bool GetCocInfoBySnResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public string[] info;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=2)]
        public string errorMessage;
        
        public GetCocInfoBySnResponse() {
        }
        
        public GetCocInfoBySnResponse(bool GetCocInfoBySnResult, string[] info, string errorMessage) {
            this.GetCocInfoBySnResult = GetCocInfoBySnResult;
            this.info = info;
            this.errorMessage = errorMessage;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IMesServiceChannel : BIModel.MesService.IMesService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MesServiceClient : System.ServiceModel.ClientBase<BIModel.MesService.IMesService>, BIModel.MesService.IMesService {
        
        public MesServiceClient() {
        }
        
        public MesServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public MesServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MesServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MesServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        BIModel.MesService.GetWorkStepResponse BIModel.MesService.IMesService.GetWorkStep(BIModel.MesService.GetWorkStepRequest request) {
            return base.Channel.GetWorkStep(request);
        }
        
        public bool GetWorkStep(string sn, out string workStep, out string errorMessage) {
            BIModel.MesService.GetWorkStepRequest inValue = new BIModel.MesService.GetWorkStepRequest();
            inValue.sn = sn;
            BIModel.MesService.GetWorkStepResponse retVal = ((BIModel.MesService.IMesService)(this)).GetWorkStep(inValue);
            workStep = retVal.workStep;
            errorMessage = retVal.errorMessage;
            return retVal.GetWorkStepResult;
        }
        
        public System.Threading.Tasks.Task<BIModel.MesService.GetWorkStepResponse> GetWorkStepAsync(BIModel.MesService.GetWorkStepRequest request) {
            return base.Channel.GetWorkStepAsync(request);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        BIModel.MesService.MoveStandardResponse BIModel.MesService.IMesService.MoveStandard(BIModel.MesService.MoveStandardRequest request) {
            return base.Channel.MoveStandard(request);
        }
        
        public bool MoveStandard(string sn, out string errorMessage) {
            BIModel.MesService.MoveStandardRequest inValue = new BIModel.MesService.MoveStandardRequest();
            inValue.sn = sn;
            BIModel.MesService.MoveStandardResponse retVal = ((BIModel.MesService.IMesService)(this)).MoveStandard(inValue);
            errorMessage = retVal.errorMessage;
            return retVal.MoveStandardResult;
        }
        
        public System.Threading.Tasks.Task<BIModel.MesService.MoveStandardResponse> MoveStandardAsync(BIModel.MesService.MoveStandardRequest request) {
            return base.Channel.MoveStandardAsync(request);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        BIModel.MesService.HoldResponse BIModel.MesService.IMesService.Hold(BIModel.MesService.HoldRequest request) {
            return base.Channel.Hold(request);
        }
        
        public bool Hold(string sn, string reason, out string errorMessage) {
            BIModel.MesService.HoldRequest inValue = new BIModel.MesService.HoldRequest();
            inValue.sn = sn;
            inValue.reason = reason;
            BIModel.MesService.HoldResponse retVal = ((BIModel.MesService.IMesService)(this)).Hold(inValue);
            errorMessage = retVal.errorMessage;
            return retVal.HoldResult;
        }
        
        public System.Threading.Tasks.Task<BIModel.MesService.HoldResponse> HoldAsync(BIModel.MesService.HoldRequest request) {
            return base.Channel.HoldAsync(request);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        BIModel.MesService.GetStepStateResponse BIModel.MesService.IMesService.GetStepState(BIModel.MesService.GetStepStateRequest request) {
            return base.Channel.GetStepState(request);
        }
        
        public bool GetStepState(string sn, out string StepState, out string errorMessage) {
            BIModel.MesService.GetStepStateRequest inValue = new BIModel.MesService.GetStepStateRequest();
            inValue.sn = sn;
            BIModel.MesService.GetStepStateResponse retVal = ((BIModel.MesService.IMesService)(this)).GetStepState(inValue);
            StepState = retVal.StepState;
            errorMessage = retVal.errorMessage;
            return retVal.GetStepStateResult;
        }
        
        public System.Threading.Tasks.Task<BIModel.MesService.GetStepStateResponse> GetStepStateAsync(BIModel.MesService.GetStepStateRequest request) {
            return base.Channel.GetStepStateAsync(request);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        BIModel.MesService.GetCocInfoBySnResponse BIModel.MesService.IMesService.GetCocInfoBySn(BIModel.MesService.GetCocInfoBySnRequest request) {
            return base.Channel.GetCocInfoBySn(request);
        }
        
        public bool GetCocInfoBySn(string sn, out string[] info, out string errorMessage) {
            BIModel.MesService.GetCocInfoBySnRequest inValue = new BIModel.MesService.GetCocInfoBySnRequest();
            inValue.sn = sn;
            BIModel.MesService.GetCocInfoBySnResponse retVal = ((BIModel.MesService.IMesService)(this)).GetCocInfoBySn(inValue);
            info = retVal.info;
            errorMessage = retVal.errorMessage;
            return retVal.GetCocInfoBySnResult;
        }
        
        public System.Threading.Tasks.Task<BIModel.MesService.GetCocInfoBySnResponse> GetCocInfoBySnAsync(BIModel.MesService.GetCocInfoBySnRequest request) {
            return base.Channel.GetCocInfoBySnAsync(request);
        }
    }
}