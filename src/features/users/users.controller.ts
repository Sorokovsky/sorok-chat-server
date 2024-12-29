import { Body, Controller, Post, UploadedFile, UseInterceptors } from "@nestjs/common";
import { UsersService } from "./users.service";
import { SwaggerFile } from "../../core/decorators/swagger-file.decorator";
import {
  CreateUserDto,
  CreateUserDtoWithoutAvatar,
} from "../../core/contracts/dto/user/create-user.dto";
import { FileInterceptor } from "@nestjs/platform-express";
import { ApiBadRequestResponse, ApiCreatedResponse } from "@nestjs/swagger";
import { ErrorDto } from "../../core/contracts/dto/error.dto";
import { GetUserDto } from "../../core/contracts/dto/user/get-user.dto";

@Controller("users")
export class UsersController {
  constructor(private readonly usersService: UsersService) {}

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
}
