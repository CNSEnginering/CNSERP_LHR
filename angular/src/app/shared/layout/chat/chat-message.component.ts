import { Component, Input, OnInit } from '@angular/core';
import { ChatMessageDto, ChatServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppConsts } from 'shared/AppConsts';
import { UtilsService } from '@abp/utils/utils.service';

@Component({
    selector: 'chat-message',
    templateUrl: './chat-message.component.html'
})
export class ChatMessageComponent implements OnInit {

    @Input()
    message: ChatMessageDto;

    chatMessage: string;
    chatMessageType: string;
    fileName: string;
    fileContentType: string;

    constructor(private _chatService: ChatServiceProxy) {
    }

    ngOnInit(): void {
        this.setChatMessageType();
    }

    private setChatMessageType(): void {
        let encryptedAuthToken = new UtilsService().getCookieValue(AppConsts.authorization.encrptedAuthTokenName);

        if (this.message.message.startsWith('[image]')) {
            this.chatMessageType = 'image';

            let image = JSON.parse(this.message.message.substring('[image]'.length));
            this.chatMessage = AppConsts.remoteServiceBaseUrl +
                '/Chat/GetUploadedObject?fileId=' +
                image.id +
                '&fileName=' +
                image.name +
                '&contentType=' +
                image.contentType + '&' + AppConsts.authorization.encrptedAuthTokenName + '=' + encodeURIComponent(encryptedAuthToken);

        } else if (this.message.message.startsWith('[file]')) {
            this.chatMessageType = 'file';

            let file = JSON.parse(this.message.message.substring('[file]'.length));
            this.chatMessage = AppConsts.remoteServiceBaseUrl +
                '/Chat/GetUploadedObject?fileId=' +
                file.id +
                '&fileName=' +
                file.name +
                '&contentType=' +
                file.contentType + '&' + AppConsts.authorization.encrptedAuthTokenName + '=' + encodeURIComponent(encryptedAuthToken);

            this.fileName = file.name;
        } else if (this.message.message.startsWith('[link]')) {
            this.chatMessageType = 'link';
            let linkMessage = JSON.parse(this.message.message.substring('[link]'.length));
            this.chatMessage = linkMessage.message == null ? '' : linkMessage.message;
        } else {
            this.chatMessageType = 'text';
            this.chatMessage = this.message.message;
        }
    }

}
