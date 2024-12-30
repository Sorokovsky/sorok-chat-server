import {
  Body,
  Controller,
  Delete, HttpCode, HttpStatus,
  Post,
  Res,
  UploadedFile,
  UseInterceptors,
} from "@nestjs/common";
import { AuthService } from "./auth.service";
import { GetUserDto } from "../../core/contracts/dto/user/get-user.dto";
import { SwaggerFile } from "../../core/decorators/swagger-file.decorator";
import { FileInterceptor } from "@nestjs/platform-express";
import {
  ApiBadRequestResponse,
  ApiCreatedResponse,
  ApiNoContentResponse,
  ApiUnauthorizedResponse,
} from "@nestjs/swagger";
import { ErrorDto } from "../../core/contracts/dto/error.dto";
import {
  CreateUserDto,
  CreateUserDtoWithoutAvatar,
} from "../../core/contracts/dto/user/create-user.dto";
import { Response } from "express";
import { Auth } from "../../core/decorators/auth.decorator";

@Controller("auth")
export class AuthController {
  constructor(private readonly authService: AuthService) {}

  @Post("register")
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
  public async register(
    @Res({ passthrough: true }) response: Response,
    @Body() user: CreateUserDtoWithoutAvatar,
    @UploadedFile() avatar?: Express.Multer.File,
  ): Promise<GetUserDto> {
    return await this.authService.register(response, user, avatar);
  }

  @Auth()
  @Delete("logout")
  @HttpCode(HttpStatus.NO_CONTENT)
  @ApiNoContentResponse({
    description: "Logged out successfully",
  })
  @ApiUnauthorizedResponse({
    description: "User not authenticated",
    type: ErrorDto,
  })
  public async logout(
    @Res({ passthrough: true }) response: Response,
  ): Promise<void> {
    return await this.authService.logout(response);
  }
}
