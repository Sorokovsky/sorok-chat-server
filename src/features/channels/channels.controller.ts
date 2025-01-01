import {
  Body,
  Controller,
  Delete,
  Get,
  Param,
  ParseIntPipe,
  Patch,
  Post,
  UploadedFiles,
  UseInterceptors,
} from "@nestjs/common";
import { ChannelsService } from "@features/channels/channels.service";
import { Auth } from "@decorators/auth.decorator";
import {
  ApiCreatedResponse,
  ApiNotFoundResponse,
  ApiOkResponse,
} from "@nestjs/swagger";
import { GetChannelDto } from "@contracts/dto/channel/get-channel.dto";
import { CurrentUser } from "@decorators/current-user.decorator";
import {
  CreateChannelDto,
  CreateChannelDtoWithoutFiles,
} from "@contracts/dto/channel/create-channel.dto";
import { SwaggerFile } from "@decorators/swagger-file.decorator";
import { FileFieldsInterceptor } from "@nestjs/platform-express";
import { ErrorDto } from "@contracts/dto/error.dto";
import {
  UpdateChannelDto,
  UpdateChannelDtoWithoutFiles,
} from "@contracts/dto/channel/update-channel.dto";

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
  @UseInterceptors(
    FileFieldsInterceptor([
      { name: "avatar", maxCount: 1 },
      { name: "image", maxCount: 1 },
    ]),
  )
  public async createChannel(
    @CurrentUser("id") userId: number,
    @Body() channel: CreateChannelDtoWithoutFiles,
    @UploadedFiles()
    files: { avatar?: Express.Multer.File[]; image?: Express.Multer.File[] },
  ): Promise<GetChannelDto> {
    return await this.channelsService.createChannel(
      userId,
      channel,
      files.avatar?.[0],
      files.image?.[0],
    );
  }

  @Auth()
  @Patch("connect/:userId/:chatId")
  @ApiOkResponse({
    type: GetChannelDto,
    description: "Connected successfully",
  })
  @ApiNotFoundResponse({
    description: "Channel not found",
    type: ErrorDto,
  })
  public async connectUser(
    @Param("userId", new ParseIntPipe()) userId: number,
    @Param("chatId", new ParseIntPipe()) chatId: number,
  ): Promise<GetChannelDto> {
    return await this.channelsService.connectUser(userId, chatId);
  }

  @Auth()
  @Patch("connect-self/:chatId")
  @ApiOkResponse({
    type: GetChannelDto,
    description: "Connected successfully",
  })
  @ApiNotFoundResponse({
    description: "Channel not found",
    type: ErrorDto,
  })
  public async connectSelfUser(
    @CurrentUser("id") userId: number,
    @Param("chatId", new ParseIntPipe()) chatId: number,
  ): Promise<GetChannelDto> {
    return await this.channelsService.connectUser(userId, chatId);
  }

  @Auth()
  @Patch("disconnect/:userId/:chatId")
  @ApiOkResponse({
    type: GetChannelDto,
    description: "Connected successfully",
  })
  @ApiNotFoundResponse({
    description: "Channel not found",
    type: ErrorDto,
  })
  public async disconnectUser(
    @Param("userId", new ParseIntPipe()) userId: number,
    @Param("chatId", new ParseIntPipe()) chatId: number,
  ): Promise<GetChannelDto> {
    return await this.channelsService.disconnectUser(userId, chatId);
  }

  @Auth()
  @Patch("disconnect-self/:chatId")
  @ApiOkResponse({
    type: GetChannelDto,
    description: "Connected successfully",
  })
  @ApiNotFoundResponse({
    description: "Channel not found",
    type: ErrorDto,
  })
  public async disconnectSelfUser(
    @CurrentUser("id") userId: number,
    @Param("chatId", new ParseIntPipe()) chatId: number,
  ): Promise<GetChannelDto> {
    return await this.channelsService.disconnectUser(userId, chatId);
  }

  @Auth()
  @Patch(":id")
  @SwaggerFile(UpdateChannelDto)
  @UseInterceptors(
    FileFieldsInterceptor([
      { name: "avatar", maxCount: 1 },
      { name: "image", maxCount: 1 },
    ]),
  )
  @ApiOkResponse({
    type: GetChannelDto,
    description: "Updated successfully",
  })
  @ApiNotFoundResponse({
    description: "Not found",
    type: ErrorDto,
  })
  public async update(
    @Param("id", new ParseIntPipe()) id: number,
    @Body() channel: UpdateChannelDtoWithoutFiles,
    @UploadedFiles()
    files: { avatar?: Express.Multer.File[]; image?: Express.Multer.File[] },
  ): Promise<GetChannelDto> {
    return await this.channelsService.update(
      id,
      channel,
      files.avatar?.[0],
      files.image?.[0],
    );
  }

  @Delete(":id")
  @ApiOkResponse({ description: "Deleted successfully", type: GetChannelDto })
  @ApiNotFoundResponse({
    description: "Not found",
    type: ErrorDto,
  })
  public async delete(
    @Param("id", new ParseIntPipe()) id: number,
  ): Promise<GetChannelDto> {
    return await this.channelsService.delete(id);
  }
}
