import {
  Body,
  Controller,
  Delete,
  Get,
  Param,
  ParseIntPipe,
  Patch,
  Post,
  UploadedFile,
  UseInterceptors,
} from "@nestjs/common";
import { UsersService } from "./users.service";
import { SwaggerFile } from "../../core/decorators/swagger-file.decorator";
import {
  CreateUserDto,
  CreateUserDtoWithoutAvatar,
} from "../../core/contracts/dto/user/create-user.dto";
import { FileInterceptor } from "@nestjs/platform-express";
import {
  ApiBadRequestResponse,
  ApiCreatedResponse,
  ApiOkResponse,
} from "@nestjs/swagger";
import { ErrorDto } from "../../core/contracts/dto/error.dto";
import { GetUserDto } from "../../core/contracts/dto/user/get-user.dto";
import {
  UpdateUserDto,
  UpdateUserDtoWithoutAvatar,
} from "../../core/contracts/dto/user/update-user.dto";
import { Auth } from "../../core/decorators/auth.decorator";
import { CurrentUser } from "../../core/decorators/current-user.decorator";

@Controller("users")
export class UsersController {
  constructor(private readonly usersService: UsersService) {}

  @Auth()
  @Get("by-id/:id")
  @ApiOkResponse({
    type: GetUserDto,
    description: "Found user successfully",
  })
  @ApiBadRequestResponse({
    description: "User not found",
    type: ErrorDto,
  })
  public async getById(
    @Param("id", new ParseIntPipe()) id: number,
  ): Promise<GetUserDto> {
    return await this.usersService.getById(id, true);
  }

  @Auth()
  @Get("get-me")
  @ApiOkResponse({
    type: GetUserDto,
    description: "Found user successfully",
  })
  @ApiBadRequestResponse({
    description: "User not found",
    type: ErrorDto,
  })
  public async getMe(@CurrentUser("id") id: number): Promise<GetUserDto> {
    return await this.usersService.getById(id, true);
  }

  @Post()
  @SwaggerFile(CreateUserDto)
  @UseInterceptors(FileInterceptor("avatar"))
  @ApiCreatedResponse({
    type: GetUserDto,
    description: "Created user successfully",
  })
  @ApiBadRequestResponse({
    description: "User already exists",
    type: ErrorDto,
  })
  public async create(
    @Body() user: CreateUserDtoWithoutAvatar,
    @UploadedFile() avatar?: Express.Multer.File,
  ): Promise<GetUserDto> {
    return await this.usersService.create(user, avatar);
  }

  @Auth()
  @SwaggerFile(UpdateUserDto)
  @UseInterceptors(FileInterceptor("avatar"))
  @ApiOkResponse({
    type: GetUserDto,
    description: "Updated user successfully",
  })
  @ApiBadRequestResponse({
    description: "User not found",
    type: ErrorDto,
  })
  @Patch()
  public async update(
    @CurrentUser("id") id: number,
    @Body() newest: UpdateUserDtoWithoutAvatar,
    @UploadedFile() avatar?: Express.Multer.File,
  ): Promise<GetUserDto> {
    return await this.usersService.update(id, newest, avatar);
  }

  @Auth()
  @ApiOkResponse({
    type: GetUserDto,
    description: "Deleted user successfully",
  })
  @ApiBadRequestResponse({
    description: "User not found",
    type: ErrorDto,
  })
  @Delete()
  public async delete(@CurrentUser("id") id: number): Promise<GetUserDto> {
    return await this.usersService.delete(id);
  }
}
