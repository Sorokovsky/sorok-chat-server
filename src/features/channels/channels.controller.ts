import {
  Body,
  Controller,
  Get,
  Post,
  UploadedFile,
  UseInterceptors,
} from "@nestjs/common";
import { ChannelsService } from "@features/channels/channels.service";
import { Auth } from "@decorators/auth.decorator";
import { ApiCreatedResponse, ApiOkResponse } from "@nestjs/swagger";
import { GetChannelDto } from "@contracts/dto/channel/get-channel.dto";
import { CurrentUser } from "@decorators/current-user.decorator";
import {
  CreateChannelDto,
  CreateChannelDtoWithoutAvatar,
} from "@contracts/dto/channel/create-channel.dto";
import { SwaggerFile } from "@decorators/swagger-file.decorator";
import { FileInterceptor } from "@nestjs/platform-express";

@Controller("channels")
export class ChannelsController {
  constructor(private readonly channelsService: ChannelsService) {}

  @Get()
  @Auth()
  @ApiOkResponse({
    type: [GetChannelDto],
  })
  public async getChannels(
    @CurrentUser("id") userId: number,
  ): Promise<GetChannelDto[]> {
    return await this.channelsService.getChannels(userId);
  }

  @Post()
  @Auth()
  @SwaggerFile(CreateChannelDto)
  @ApiCreatedResponse({
    type: GetChannelDto,
  })
  @UseInterceptors(FileInterceptor("avatar"))
  public async createChannel(
    @CurrentUser("id") userId: number,
    @Body() channel: CreateChannelDtoWithoutAvatar,
    @UploadedFile() avatar?: Express.Multer.File,
  ): Promise<GetChannelDto> {
    return await this.channelsService.createChannel(userId, channel, avatar);
  }
}
